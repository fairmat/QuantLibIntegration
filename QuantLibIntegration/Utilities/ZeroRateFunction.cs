/* Copyright (C) 2013 Fairmat SRL (info@fairmat.com, http://www.fairmat.com/)
 * Author(s): Matteo Tesser (matteo.tesser@fairmat.com)
 *            
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLNet;
using DVPLI;
using DVPLDOM;
namespace QuantLibIntegration.Utilities
{
    


    /// <summary>
    /// Defines a Quantlib ZeroYieldStructure binded to Fairmat data
    /// </summary>
    public class ZeroRateFunction : ZeroYieldStructure
    {
        IFunction function;
        double maxT;
        Date  date;        

        public ZeroRateFunction(DateTime refDate,IFunction function,double maxT)
        : base(new ActualActual())
        {
            this.function = function;
            this.date = new Date(refDate);
        }

        public ZeroRateFunction(DateTime refDate, DVPLI.Vector x, DVPLI.Vector y)
        : base(new ActualActual())
        {
            this.date = new Date(refDate);
            DVPLDOM.PFunction pf = new PFunction(null);
            double[,] zrvalue = (double[,])ArrayHelper.Concat(x.ToArray(), y.ToArray());
            pf.Expr = zrvalue;
            this.function = pf;
            this.maxT = x.Max()+130;// add  years in order to handle date approximations
        }

        protected override double zeroYieldImpl(double t)
        {
            return function.Evaluate(t);
        }
        public override double maxTime()
        {
            return this.maxT;
        }
        public override Date referenceDate()
        {
            return date;
        }
    }
}
