using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Image
    {
        // Type: string
        // The URI(or IRI) of the image.
        // Required: No
        public string uri { get; set; } = String.Empty;

        // Type: string
        // The image’s media type.This field MUST be defined when bufferView is defined.
        // Required: No
        public string mimeType { get; set; } = "application/text";

        // Type: integer
        // The index of the bufferView that contains the image.This field MUST NOT be defined when uri is defined.
        // Required: No
        public int bufferView { get; set; } = -1;

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = String.Empty;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public Extra? extras { get; set; } = null;
        /*
         * **********************************************************
         */
        private bool isLocked = false;
        public void Lock()
        {
            isLocked = true;
        }
        public void Unlock()
        {
            isLocked = false;
        }

    }
}
