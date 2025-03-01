namespace Porkbun {

    /// <summary>
    /// Represents different types of DNS records.
    /// </summary>
    public enum DnsRecordType {
        /// <summary>IPv4 address record</summary>
        A,
        /// <summary>IPv6 address record</summary>
        AAAA,
        /// <summary>Canonical name record</summary>
        CNAME,
        /// <summary>Mail exchange record</summary>
        MX,
        /// <summary>Text record</summary>
        TXT,
        /// <summary>Name server record</summary>
        NS,
        /// <summary>Service locator record</summary>
        SRV,
        /// <summary>Pointer record</summary>
        PTR,
        /// <summary>Start of authority record</summary>
        SOA,
        /// <summary>Certification authority authorization record</summary>
        CAA,
        /// <summary>Sender policy framework record</summary>
        SPF,
        /// <summary>Name authority pointer record</summary>
        NAPTR
    }

}