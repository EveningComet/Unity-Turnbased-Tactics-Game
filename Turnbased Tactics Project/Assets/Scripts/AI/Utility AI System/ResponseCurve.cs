using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TurnbasedGame.AI
{
    public enum CurveType
    {
        None,
        Linear,
        QuadraticOrPolynomial,
        Logistic,
        Logit
    }

    /// <summary>
    /// Math related stuff that will help a <see cref="Consideration"/> get the value it should return.
    /// </summary>
    public class ResponseCurve
    {
        [JsonProperty("curveType")]
        [JsonConverter(typeof(StringEnumConverter))]
        private CurveType curveType;

        [JsonProperty("m")] private float m; // Slope
        [JsonProperty("k")] private float k; // Exponent
        [JsonProperty("b")] private float b; // y-shift
        [JsonProperty("c")] private float c; // x-shift

        public ResponseCurve(CurveType cType, float slope, float exponent = 1, float yShift = 0, float xShift = 0)
        {
            curveType = cType;
            m = slope;
            k = exponent;
            b = yShift;
            c = xShift;
        }

        /// <summary>
        /// Get the curve output.
        /// </summary>
        /// <param name="x">The value being passed in.</param>
        public float Calculate(float x)
        {
            float y = 0f;
            switch (curveType)
            {
                case CurveType.Linear:
                    y = m * (x - c) + b;
                    break;
                case CurveType.QuadraticOrPolynomial:
                    y = m * (float)Math.Pow(x - c, k) + b;
                    break;
                default:
                    return 0f;
            }
            return y;
        }

        #region
        public override string ToString()
        {
            return string.Format("Curve Type ({0}), m or slope ({1}), k or exponent ({2}), b or y-shift ({3})," +
                " c or x-shift ({4}).", curveType, m, k, b, c);
        }
        #endregion
    }
}
