using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SharpNoise.Modules
{
    /// <summary>
    /// Noise module that maps the output value from a source module onto a
    /// terrace-forming curve.
    /// </summary>
    /// <remarks>
    /// This noise module maps the output value from the source module onto a
    /// terrace-forming curve.  The start of this curve has a slope of zero;
    /// its slope then smoothly increases.  This curve also contains
    /// control points which resets the slope to zero at that point,
    /// producing a "terracing" effect.
    /// 
    /// To add a control point to this noise module, call the
    /// <see cref="AddControlPoint"/> method.
    ///
    /// An application must add a minimum of two control points to the curve.
    /// If this is not done, the <see cref="GetValue"/> method fails.  The control points
    /// can have any value, although no two control points can have the same
    /// value.  There is no limit to the number of control points that can be
    /// added to the curve.
    ///
    /// This noise module clamps the output value from the source module if
    /// that value is less than the value of the lowest control point or
    /// greater than the value of the highest control point.
    ///
    /// This noise module is often used to generate terrain features such as
    /// your stereotypical desert canyon.
    ///
    /// This noise module requires one source module.
    /// </remarks>
    [Serializable]
    public class Terrace : Module
    {
        public List<Vector2> ControlPoints;

        /// <summary>
        /// Enables or disables the inversion of the terrace-forming curve
        /// between the control points.
        /// </summary>
        public bool InvertTerraces { get; set; } = false;

        /// <summary>
        /// Gets or sets all ControlPoints in the Module
        /// </summary>

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
        public Terrace()
            : base(1)
        {
            ControlPoints = new List<Vector2>();
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
            double sourceModuleValue = SourceModules[0].GetValue(x, y, z);

            // Find the first element in the control point array that has a value
            // larger than the output value from the source module.
            int indexPos;
            for (indexPos = 0; indexPos < ControlPoints.Count; indexPos++)
            {
                if (sourceModuleValue < ControlPoints[indexPos].x)
                    break;
            }

            // Find the two nearest control points so that we can map their values
            // onto a quadratic curve.
            var index0 = NoiseMath.Clamp(indexPos - 1, 0, ControlPoints.Count - 1);
            var index1 = NoiseMath.Clamp(indexPos, 0, ControlPoints.Count - 1);

            // If some control points are missing (which occurs if the output value from
            // the source module is greater than the largest value or less than the
            // smallest value of the control point array), get the value of the nearest
            // control point and exit now.
            if (index0 == index1)
                return ControlPoints[index1].x;

            // Compute the alpha value used for linear interpolation.
            var value0 = ControlPoints[index0].x;
            var value1 = ControlPoints[index1].x;
            var alpha = (sourceModuleValue - value0) / (value1 - value0);
            if (InvertTerraces)
            {
                alpha = 1.0 - alpha;
                NoiseMath.Swap(ref value0, ref value1);
            }

            // Squaring the alpha produces the terrace effect.
            alpha *= alpha;

            // Now perform the linear interpolation given the alpha value.
            return NoiseMath.Linear(value0, value1, alpha);
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
