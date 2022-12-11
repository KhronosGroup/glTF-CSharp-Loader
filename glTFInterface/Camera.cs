using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    internal class Camera
    {
        /*
         * 
orthographic

camera.orthographic

An orthographic camera containing properties to create an orthographic projection matrix. This property MUST NOT be defined when perspective is defined.

No

perspective

camera.perspective

A perspective camera containing properties to create a perspective projection matrix. This property MUST NOT be defined when orthographic is defined.

No

type

string

Specifies if the camera uses a perspective or orthographic projection.

 Yes

name

string

The user-defined name of this object.

No

extensions

extension

JSON object with extension-specific objects.

No

extras

extras

Application-specific data.

No         * 
         */
    }
    public class OrthographicCamera
    {
        /*
         * 
xmag

number

The floating-point horizontal magnification of the view. This value MUST NOT be equal to zero. This value SHOULD NOT be negative.

 Yes

ymag

number

The floating-point vertical magnification of the view. This value MUST NOT be equal to zero. This value SHOULD NOT be negative.

 Yes

zfar

number

The floating-point distance to the far clipping plane. This value MUST NOT be equal to zero. zfar MUST be greater than znear.

 Yes

znear

number

The floating-point distance to the near clipping plane.

 Yes

extensions

extension

JSON object with extension-specific objects.

No

extras

extras

Application-specific data.

No
         * 
         */

    }
    public class PerspectiveCamera
    {
        /*
         * 
        aspectRatio

        number

        The floating-point aspect ratio of the field of view.

        No

        yfov

        number

        The floating-point vertical field of view in radians. This value SHOULD be less than π.

         Yes

        zfar

        number

        The floating-point distance to the far clipping plane.

        No

        znear

        number

        The floating-point distance to the near clipping plane.

         Yes

        extensions

        extension

        JSON object with extension-specific objects.

        No

        extras

        extras

        Application-specific data.

        No
         * 
         */

    }
}
