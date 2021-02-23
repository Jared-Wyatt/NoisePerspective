using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SharpNoise.Modules
{
    /// <summary>
    /// Noise module that maps the output value from a source module onto an
    /// arbitrary function curve.
    /// </summary>
    /// <remarks>
    /// This noise module maps the output value from the source module onto an
    /// application-defined curve.  This curve is defined by a number of
    /// control points; each control point has an input value
    /// that maps to an output value.
    ///
    /// To add the control points to this curve, call the <see cref="AddControlPoint"/>
    /// method.
    ///
    /// Since this curve is a cubic spline, an application must add a minimum
    /// of four control points to the curve.  If this is not done, the
    /// <see cref="GetValue"/> method fails.  Each control point can have any input and
    /// output value, although no two control points can have the same input
    /// value.  There is no limit to the number of control points that can be
    /// added to the curve.  
    ///
    /// This noise module requires one source module.
    /// </remarks>
    [Serializable]
    public class Curve : Module
    {
        /// <summary>
        /// This structure defines a control point.
        /// Control points are used for defining splines.
        /// </summary>
       

        public List<Vector2> ControlPoints;

        /// <summary>
        /// Gets or sets the first source module
        /// </summary>
        public Module Source0
        {
            get { return SourceModules[0]; }
            set { SourceModules[0] = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Curve()
            : base(1)
        {
            this.ControlPoints = new List<Vector2>();
        }

        /// <summary>
        /// See the documentation on the base class.
        /// <seealso cref="Module"/>
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <returns>Returns the computed value</returns>
        public override double GetValue(double x, double y, double z)
        {
            // Get the output value from the source module.
            var sourceValue = SourceModules[0].GetValue(x, y, z);

            // Find the first element in the control point array that has an input value
            // larger than the output value from the source module.
            int indexPos;
            for (indexPos = 0; indexPos < ControlPoints.Count; indexPos++)
            {
                if (sourceValue < ControlPoints[indexPos].x)
                    break;
            }

            // Find the four nearest control points so that we can perform cubic
            // interpolation.
            var index0 = NoiseMath.Clamp(indexPos - 2, 0, ControlPoints.Count - 1);
            var index1 = NoiseMath.Clamp(indexPos - 1, 0, ControlPoints.Count - 1);
            var index2 = NoiseMath.Clamp(indexPos, 0, ControlPoints.Count - 1);
            var index3 = NoiseMath.Clamp(indexPos + 1, 0, ControlPoints.Count - 1);

            // If some control points are missing (which occurs if the value from the
            // source module is greater than the largest input value or less than the
            // smallest input value of the control point array), get the corresponding
            // output value of the nearest control point and exit now.
            if (index1 == index2)
                return ControlPoints[index1].y;

            // Compute the alpha value used for cubic interpolation.
            var input0 = ControlPoints[index1].x;
            var input1 = ControlPoints[index2].x;
            var alpha = (sourceValue - input0) / (input1 - input0);

            // Now perform the cubic interpolation given the alpha value.
            return NoiseMath.Cubic(
              ControlPoints[index0].y,
              ControlPoints[index1].y,
              ControlPoints[index2].y,
              ControlPoints[index3].y,
              alpha);
        }

        public override void SetData(NoisePerspective.Data.NodeData nodeData)
        {
            base.SetData(nodeData);

            ControlPoints = nodeData.controlPoints;
        }

        public override void SetSourceModules(Module[] sourceModules)
        {
            Source0 = sourceModules[0];
        }
    }
}
