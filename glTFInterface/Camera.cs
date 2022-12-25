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
        public OrthographicCamera? orthographic { get; set; } = null;

        // Type: camera.perspective
        // A perspective camera containing properties to create a perspective projection matrix.
        // This property MUST NOT be defined when orthographic is defined.
        // Required: No; ust not be present if orthographic is defined
        public PerspectiveCamera? perspective { get; set; } = null;

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
        public List<Extra>? extras { get; set; } = null;
    }
    public class OrthographicCamera
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
        public List<Extra>? extras { get; set; } = null;
    }
    public class PerspectiveCamera
    {
        // Type:  number
        // The floating-point aspect ratio of the field of view.
        // Required: No
        public double aspectRatio { get; set; } = 1.0;

        // Type: number
        // The floating-point vertical field of view in radians.
        // This value SHOULD be less than π.
        // Required: Yes
        public double yfov { get; set; } = 1.0;

        // Type: number
        //  The floating-point horizontal magnification of the view.T
        //  his value MUST NOT be equal to zero.
        //  This value SHOULD NOT be negative.
        // Required: Yes
        public double xmag { get; set; } = 1.0;

        // Type: number
        // The floating-point distance to the far clipping plane.
        // Required: No
        public double zfar { get; set; } = double.MaxValue;

        // Type: number
        // The floating-point distance to the near clipping plane.
        // Required: Yes
        public double znear { get; set; } = double.MinValue;

        // Type: extension
        // JSON object with extension-specific objects.
        // Required: No
        public System.Collections.Generic.Dictionary<string, object>? extensions { get; set; } = null;

        // Type: extras
        // Application-specific data.
        // Required: No
        public List<Extra>? extras { get; set; } = null;
    }
}
