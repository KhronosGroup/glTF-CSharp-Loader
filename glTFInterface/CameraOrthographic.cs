using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class CameraOrthographic
    {
        // Type: number
        // The floating-point vertical magnification of the view.
        // This value MUST NOT be equal to zero.
        // This value SHOULD NOT be negative.
        // Required: Yes
        public float xmag { get; set; } = 1.0f;

        // Type: number
        // The floating-point distance to the far clipping plane.
        // Required: Yes
        public float zfar { get; set; } = float.MaxValue;

        // Type: number
        // The floating-point distance to the near clipping plane.
        // Required: Yes
        public float znear { get; set; } = float.MinValue;

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
