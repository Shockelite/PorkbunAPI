namespace Porkbun {

    /// <summary>
    /// Represents a label associated with a domain.
    /// </summary>
    public class DomainLabel {
        /// <summary>
        /// The unique ID of the label.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// The title of the label.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// The color of the label in hexadecimal format.
        /// </summary>
        public string color { get; set; }

        public override string ToString() =>
            $"Label {{ id: {id ?? "null"}, title: {title ?? "null"}, color: {color ?? "null"} }}";
    }

}