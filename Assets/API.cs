using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
#if NET48 || NET481 
using Newtonsoft.Json;
#endif

namespace Porkbun {
    public class API {

        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Gets the API key used for authentication with the Porkbun API.
        /// </summary>
        /// <remarks>
        /// This key is required for all API requests to authenticate the user.
        /// </remarks>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the API secret key used for authentication with the Porkbun API.
        /// </summary>
        /// <remarks>
        /// This secret is paired with the API key to ensure secure API requests.
        /// Keep it confidential and do not expose it publicly.
        /// </remarks>
        public string Secret { get; private set; }

        /// <summary>
        /// Constructor for the Porkbun class, requiring both API key and secret.
        /// Ensures that neither the key nor the secret is null or empty.
        /// </summary>
        /// <param name="key">The API key used for authentication with Porkbun.</param>
        /// <param name="secret">The API secret key used for authentication with Porkbun.</param>
        /// <exception cref="ArgumentNullException">Thrown when either the key or secret is null or empty.</exception>
        public API(string key, string secret) {
            if (string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException(nameof(key), "API key cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(secret)) {
                throw new ArgumentNullException(nameof(secret), "API secret cannot be null or empty.");
            }

            Key = key;
            Secret = secret;
        }

        /// <summary>
        /// Sends a request to the Porkbun API and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type of response object expected.</typeparam>
        /// <param name="url">The API endpoint URL.</param>
        /// <param name="requestBody">The request payload.</param>
        /// <returns>The deserialized response object of type T, or default(T) if an error occurs.</returns>
        private static async Task<T> SendRequest<T>(string url, object requestBody) where T : StatusResponse, new() {
#if NET48 || NET481
            string jsonContent = JsonConvert.SerializeObject(requestBody);
#else
            string jsonContent = System.Text.Json.JsonSerializer.Serialize(requestBody);
#endif
            using (StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json")) {
                string responseBody = null;
                try {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode) {
#if NET48 || NET481
                        T obj = JsonConvert.DeserializeObject<T>(responseBody);
                        obj.message = string.Empty;
                        return obj;
#else
                        T obj = System.Text.Json.JsonSerializer.Deserialize<T>(responseBody);
                        obj.message = responseBody;
                        return obj;
#endif
                    }
                    else {
                        throw new Exception($"{{StatusCode: \"{response.StatusCode}\", Content: \"{responseBody?.Replace("\r", string.Empty).Replace("\n", string.Empty) ?? "Null"}\", IsSuccessStatusCode: {response.IsSuccessStatusCode} }}");
                    }
                }
                catch (Exception ex) {
                    if (ex.Message.Contains("Invalid domain"))
                        throw new InvalidDomainException(ex.Message);
                    if (ex.Message.Contains("Invalid API key. (001)"))
                        throw new InvalidAPIKeyException(ex.Message);
                    if (ex.Message.Contains("Invalid API key. (002)"))
                        throw new InvalidAPISecretException(ex.Message);
                    if (ex.Message.Contains("ServiceUnavailable"))
                        throw new RatelimitException(ex.Message);
                    if (ex.Message.Contains("BadRequest"))
                        throw new InvalidRequest(ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves the pricing for different domain extensions from Porkbun API.
        /// </summary>
        /// <returns>A task containing the pricing response with registration, renewal, and transfer prices for various domain types.</returns>
        public static Task<PricingResponse> Pricing() {
            string url = "https://api.porkbun.com/api/json/v3/pricing/get";
            return SendRequest<PricingResponse>(url, null);
        }

        /// <summary>
        /// Pings the Porkbun API to check status and retrieve the IP address of the requestor.
        /// </summary>
        /// <returns>A task containing the response object with the status and IP address.</returns>
        public Task<PingResponse> Ping() {
            string url = "https://api.porkbun.com/api/json/v3/ping";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<PingResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves a list of all domains associated with an account using the Porkbun API.
        /// </summary>
        /// <param name="start">The starting index for pagination (e.g., "1"). Using a default value (less than 0) will grab all domains.</param>
        /// <param name="includeLabels">Whether to include labels in the response ("yes" or "no").</param>
        /// <returns>A task containing the response object with a list of all domains.</returns>
        public async Task<DomainListResponse> DomainList(int start = -1, bool includeLabels = true) {
            string url = "https://api.porkbun.com/api/json/v3/domain/listAll";
            if (start > 0) {
                var requestBody = new {
                    apikey = this.Key,
                    secretapikey = this.Secret,
                    start = start.ToString(),
                    includeLabels = includeLabels ? "yes" : "no"
                };

                return await SendRequest<DomainListResponse>(url, requestBody);
            }
            else {
                start = 0;
                List<Domain> domainList = new List<Domain>();
                while (true) {
                    var requestBody = new {
                        apikey = this.Key,
                        secretapikey = this.Secret,
                        start = start++.ToString(),
                        includeLabels = includeLabels ? "yes" : "no"
                    };

                    DomainListResponse response = await SendRequest<DomainListResponse>(url, requestBody);
                    if (response?.domains == null || response.domains.Count() == 0)
                        break;

                    domainList.AddRange(response.domains);
                }
                return new DomainListResponse() { domains = domainList, status = domainList.Any() ? "SUCCESS" : "FAILURE" };
            }
        }

        /// <summary>
        /// Checks the availability of a domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name to check (e.g., example.com).</param>
        /// <returns>A response object containing domain availability details.</returns>
        public Task<DomainCheckResponse> DomainCheck(string domain) {
            string url = $"https://api.porkbun.com/api/json/v3/domain/checkDomain/{domain}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<DomainCheckResponse>(url, requestBody);
        }

        /// <summary>
        /// Adds a new URL forwarding record for a specified domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name (e.g., example.com).</param>
        /// <param name="name">The subdomain to forward (leave empty for root domain).</param>
        /// <param name="location">The destination URL for the forwarding.</param>
        /// <param name="type">The type of forwarding (e.g., "temporary" or "permanent").</param>
        /// <param name="includePath">Whether to include the path in the forwarding ("yes" or "no").</param>
        /// <param name="isWildcard">Whether wildcard forwarding is enabled ("yes" or "no").</param>
        /// <returns>A task containing the response object with the status of the URL forward creation.</returns>
        public Task<UrlForwardAddResponse> UrlForwardAdd(string domain, string name, string location, DomainForwardingType type, bool includePath, bool isWildcard) {
            string url = $"https://api.porkbun.com/api/json/v3/domain/addUrlForward/{domain}";

            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret,
                subdomain = name,
                location = location,
                type = type,
                includePath = includePath ? "yes" : "no",
                wildcard = isWildcard ? "yes" : "no"
            };

            return SendRequest<UrlForwardAddResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves the URL forwarding settings for a specified domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name (e.g., example.com).</param>
        /// <returns>A response object containing a list of URL forwarding records.</returns>
        public Task<UrlForwardGetResponse> UrlForwardGet(string domain) {
            string url = $"https://api.porkbun.com/api/json/v3/domain/getUrlForwarding/{domain}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<UrlForwardGetResponse>(url, requestBody);
        }

        /// <summary>
        /// Deletes the URL forwarding settings for a specified domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name (e.g., example.com).</param>
        /// <param name="id">The record ID to remove. Shoild be an integer.</param>
        /// <returns>A task containing the response object indicating success or failure.</returns>
        public Task<UrlForwardRemoveResponse> UrlForwardRemove(string domain, string id) {
            string url = $"https://api.porkbun.com/api/json/v3/domain/deleteUrlForward/{domain}/{id}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<UrlForwardRemoveResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves the nameservers associated with a domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain for which to retrieve nameservers.</param>
        /// <returns>A task containing the response object with the list of nameservers.</returns>
        public Task<NameServerGetResponse> NameServerGet(string domain) {
            string url = $"https://api.porkbun.com/api/json/v3/domain/getNs/{domain}";

            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret
            };

            return SendRequest<NameServerGetResponse>(url, requestBody);
        }

        /// <summary>
        /// Updates the nameservers for a domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain for which to update nameservers.</param>
        /// <param name="nameservers">The list of nameservers to be set for the domain.</param>
        /// <returns>A task containing the response object indicating success or failure.</returns>
        public Task<NameServerEditResponse> NameServerEdit(string domain, List<string> nameservers) {
            string url = $"https://api.porkbun.com/api/json/v3/domain/updateNs/{domain}";
            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret,
                ns = nameservers
            };
            return SendRequest<NameServerEditResponse>(url, requestBody);
        }

        /// <summary>
        /// Deletes a DNS record from Porkbun by name and type using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="type">The record type (e.g., A, CNAME, TXT).</param>
        /// <param name="name">The subdomain to delete.</param>
        /// <returns>A response object indicating success or failure.</returns>
        public Task<DnsRemoveResponse> DnsRemove(string domain, DnsRecordType type, string name) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/deleteByNameType/{domain}/{type}/{name}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<DnsRemoveResponse>(url, requestBody);
        }

        /// <summary>
        /// Deletes a DNS record from Porkbun by record id using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="recordId">The record ID to remove.</param>
        /// <returns>A response object indicating success or failure.</returns>
        public Task<DnsRemoveResponse> DnsRemove(string domain, long recordId) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/delete/{domain}/{recordId}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<DnsRemoveResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves DNS records for a given domain, optionally filtering by record ID using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="id">Optional. The specific record ID to retrieve. Should be an integer.</param>
        /// <returns>A response object containing the DNS records.</returns>
        public Task<DnsGetResponse> DnsGet(string domain, string id = "-1") {
            string url = $"https://api.porkbun.com/api/json/v3/dns/retrieve/{domain}";
            if (id != "-1") url += $"/{id}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<DnsGetResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves DNS records by name and type for a domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain for which to retrieve DNS records.</param>
        /// <param name="type">The type of DNS record (e.g., "A", "CNAME").</param>
        /// <param name="name">The subdomain for which to retrieve the DNS record.</param>
        /// <returns>A task containing the response object with the list of DNS records.</returns>
        public Task<DnsGetResponse> DnsGet(string domain, DnsRecordType type, string name) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/retrieveByNameType/{domain}/{type}/{name}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret };
            return SendRequest<DnsGetResponse>(url, requestBody);
        }

        /// <summary>
        /// Edits an existing DNS record by name using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="type">The record type (e.g., A, CNAME, TXT).</param>
        /// <param name="name">The subdomain to edit.</param>
        /// <param name="content">The new content for the DNS record.</param>
        /// <param name="ttl">The time-to-live (TTL) value in seconds.</param>
        /// <returns>A response object indicating success or failure.</returns>
        public Task<DnsEditResponse> DnsEdit(string domain, DnsRecordType type, string name, string content, int ttl = 600) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/editByNameType/{domain}/{type}/{name}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret, content = content, ttl = ttl.ToString() };
            return SendRequest<DnsEditResponse>(url, requestBody);
        }

        /// <summary>
        /// Edits an existing DNS record by ID using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="id">The record ID to edit (should be a long).</param>
        /// <param name="name">The name of the DNS record.</param>
        /// <param name="type">The record type (e.g., A, CNAME, TXT).</param>
        /// <param name="content">The new content for the DNS record.</param>
        /// <param name="ttl">The time-to-live (TTL) value in seconds.</param>
        /// <returns>A response object indicating success or failure.</returns>
        public Task<DnsEditResponse> DnsEdit(string domain, string id, string name, DnsRecordType type, string content, int ttl = 600) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/edit/{domain}/{id}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret, name = name, type = type.ToString(), content = content, ttl = ttl.ToString() };
            return SendRequest<DnsEditResponse>(url, requestBody);
        }

        /// <summary>
        /// Creates a new DNS record for the specified domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="type">The record type (e.g., A, CNAME, TXT).</param>
        /// <param name="name">The name of the DNS record (subdomain).</param>
        /// <param name="content">The content of the DNS record.</param>
        /// <param name="ttl">The time-to-live (TTL) value in seconds.</param>
        /// <returns>A response object containing the newly created record's ID.</returns>
        public Task<DnsAddResponse> DnsAdd(string domain, DnsRecordType type, string name, string content, int ttl = 600) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/create/{domain}";
            var requestBody = new { apikey = this.Key, secretapikey = this.Secret, name = name, type = type.ToString(), content = content, ttl = ttl.ToString() };
            return SendRequest<DnsAddResponse>(url, requestBody);
        }

        /// <summary>
        /// Edits an existing DNS record by name and type using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name.</param>
        /// <param name="type">The record type (e.g., A, CNAME, TXT).</param>
        /// <param name="name">The subdomain to edit.</param>
        /// <param name="content">The new content for the DNS record.</param>
        /// <param name="ttl">The time-to-live (TTL) value in seconds.</param>
        /// <returns>A response object indicating success or failure.</returns>
        public async Task<StatusResponse> DnsAddOrEdit(string domain, DnsRecordType type, string name, string content, int ttl = 600) {
            DnsGetResponse dnsRetrieveResponse = await DnsGet(domain, type, name);
            if (dnsRetrieveResponse.records.Count() == 0) {
                return await DnsAdd(domain, type, name, content, ttl);
            }
            else {
                DnsEditResponse response = null;
                for (int i = 0; i < dnsRetrieveResponse.records.Count(); i++) {
                    DnsRecord dnsRecord = dnsRetrieveResponse.records[i];
                    if (dnsRecord.content != content) {
                        response = await DnsEdit(domain, dnsRecord.id, name, type, content, ttl);
                    }
                }
                return response ?? new DnsEditResponse { status = "Unchanged" };
            }
        }

        /// <summary>
        /// Creates a DNSSEC record for a given domain using the Porkbun API.
        /// </summary>
        public Task<DnssecAddResponse> DnssecRecordAdd(string domain, string keyTag, string alg, string digestType, string digest, string maxSigLife, string keyDataFlags, string keyDataProtocol, string keyDataAlgo, string keyDataPubKey) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/createDnssecRecord/{domain}";
            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret,
                keyTag = keyTag,
                alg = alg,
                digestType = digestType,
                digest = digest,
                maxSigLife = maxSigLife,
                keyDataFlags = keyDataFlags,
                keyDataProtocol = keyDataProtocol,
                keyDataAlgo = keyDataAlgo,
                keyDataPubKey = keyDataPubKey
            };
            return SendRequest<DnssecAddResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves DNSSEC records for a given domain using the Porkbun API.
        /// </summary>
        public Task<DnssecGetResponse> DnssecRecordGet(string domain) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/getDnssecRecords/{domain}";
            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret
            };
            return SendRequest<DnssecGetResponse>(url, requestBody);
        }

        /// <summary>
        /// Deletes a DNSSEC record for a given domain using the Porkbun API.
        /// </summary>
        public Task<DnssecRemoveResponse> DeleteDnssecRecord(string domain, string keyTag) {
            string url = $"https://api.porkbun.com/api/json/v3/dns/deleteDnssecRecord/{domain}/{keyTag}";
            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret
            };
            return SendRequest<DnssecRemoveResponse>(url, requestBody);
        }

        /// <summary>
        /// Retrieves SSL certificate details for a given domain using the Porkbun API.
        /// </summary>
        /// <param name="domain">The domain name to retrieve the SSL certificate for.</param>
        /// <returns>A task containing the SSL certificate response object.</returns>
        public Task<SslGetResponse> SslGet(string domain) {
            string url = $"https://api.porkbun.com/api/json/v3/ssl/retrieve/{domain}";
            var requestBody = new {
                apikey = this.Key,
                secretapikey = this.Secret
            };
            return SendRequest<SslGetResponse>(url, requestBody);
        }

        /// <summary>
        /// Retries an API call if a <see cref="RatelimitException"/> is encountered, with a delay between attempts.
        /// </summary>
        /// <typeparam name="T">The type of the response object, which must inherit from <see cref="StatusResponse"/>.</typeparam>
        /// <param name="apiCall">The API call function to execute.</param>
        /// <param name="delay">The delay in milliseconds before retrying after a rate limit exception. Default is 5000ms.</param>
        /// <param name="attempts">The maximum number of retry attempts. Default is 3.</param>
        /// <returns>The response from the API call if successful.</returns>
        /// <exception cref="RetryExhaustion">Thrown when all retry attempts are exhausted due to repeated rate limit exceptions.</exception>
        /// <example>
        /// Example usage with the <see cref="Ping"/> method:
        /// <code>
        /// var response = await RateLimitRetry(() => _api.Ping());
        /// Console.WriteLine($"Status: {response.status}, IP: {response.yourIp}");
        /// </code>
        /// </example>
        public static async Task<T> RateLimitRetry<T>(Func<Task<T>> apiCall, int delay = 5000, int attempts = 3) where T : StatusResponse {
            while (attempts-- > 0) {
                try {
                    T response = await apiCall();
                    return response;
                }
                catch (RatelimitException) {
                    Console.WriteLine($"Warning: Rate limit reached. Retry in {delay} milliseconds.");
                    await Task.Delay(delay);
                }
                catch(Exception ex) {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            throw new RetryExhaustion("Unable to continue retrying because of ratelimit exceptions.");
        }

    }
}
