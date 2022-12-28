using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class Camera
    {
        // Type: camera.orthographic
        // An orthographic camera containing properties to create an orthographic projection matrix.
        // This property MUST NOT be defined when perspective is defined.
        // Required: No; Must not be present if perspective is defined
        public CameraOrthographic? orthographic { get; set; } = null;

        // Type: camera.perspective
        // A perspective camera containing properties to create a perspective projection matrix.
        // This property MUST NOT be defined when orthographic is defined.
        // Required: No; ust not be present if orthographic is defined
        public CameraPerspective? perspective { get; set; } = null;

        // Type: string
        // Specifies if the camera uses a perspective or orthographic projection.
        // Required: Yes; valid values are "orthographic", "perspective"
        public string type { get; set; } = string.Empty;

        // Type: string
        // The user-defined name of this object.
        // Required: No
        public string name { get; set; } = string.Empty;

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
