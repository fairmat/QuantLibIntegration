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

/*
 Copyright (C) 2010 Philippe Real (ph_real@hotmail.com)
  
 This file is part of QLNet Project http://qlnet.sourceforge.net/

 QLNet is free software: you can redistribute it and/or modify it
 under the terms of the QLNet license.  You should have received a
 copy of the license along with this program; if not, license is  
 available online at <http://qlnet.sourceforge.net/License.html>.
  
 QLNet is a based on QuantLib, a free-software/open-source library
 for financial quantitative analysts and developers - http://quantlib.org/
 The QuantLib license is available online at http://quantlib.org/license.shtml.
 
 This program is distributed in the hope that it will be useful, but WITHOUT
 ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 FOR A PARTICULAR PURPOSE.  See the license for more details.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLNet;
using Mono.Addins;

namespace QuantLibIntegration.InterestRateModels
{
    /// <summary>
    /// Calibrate HW model using QuantLib
    /// </summary>
    [Extension("/Fairmat/Estimator")]
    public class HullWhiteOneFactorCalibration: DVPLI.IEstimator, DVPLI.IMenuItemDescription
    {

        public DVPLI.EstimateRequirement[] GetRequirements(DVPLI.IEstimationSettings settings, DVPLI.EstimateQuery query)
        {
            return new DVPLI.EstimateRequirement[] { new DVPLI.EstimateRequirement(typeof(DVPLI.InterestRateMarketData)) };
        }

        public Type ProvidesTo
        {
            get { return Type.GetType("HullAndWhiteOneFactor.HW1, HullAndWhiteOneFactor"); }
        }

        public string ToolTipText
        {
            get { return "Calibrate HW1 using Swaptions (Quantlib)"; }
        }

        public string Description
        {
            get { return "Calibrate HW1 using Swaptions (Quantlib)"; }
        }

           public DVPLI.EstimationResult Estimate(List<object> data, DVPLI.IEstimationSettings settings = null, DVPLI.IController controller = null, Dictionary<string, object> properties = null)
           {
             DVPLI.InterestRateMarketData irmd = data[0] as DVPLI.InterestRateMarketData;

            //Date today = new Date(15, Month.February, 2002);
            //Date settlement = new Date(19, Month.February, 2002);
            Settings.setEvaluationDate(irmd.Date);
              


            Handle<YieldTermStructure> termStructure = new Handle<YieldTermStructure>(new Utilities.ZeroRateFunction(irmd.Date,irmd.ZRMarketDates, irmd.ZRMarket));
            
            //termStructure.link
            HullWhite model = new HullWhite(termStructure);

             
            IborIndex index = new Euribor6M(termStructure);

            IPricingEngine engine = new JamshidianSwaptionEngine(model);

            List<CalibrationHelper> swaptions = new List<CalibrationHelper>();
            for (int i = 0; i < irmd.SwapDates.Length; i++)
            {
                Quote vol = new SimpleQuote(irmd.SwapRates[i]);
                CalibrationHelper helper =
                                     new SwaptionHelper(new Period((int)irmd.SwapDates[i], TimeUnit.Years),
                                                        new Period((int)irmd.SwapDuration[i], TimeUnit.Years),
                                                        new Handle<Quote>(vol),
                                                        index,
                                                        new Period(1, TimeUnit.Years),
                                                        new Thirty360(),
                                                        new Actual360(),
                                                        termStructure,
                                                        false);
                helper.setPricingEngine(engine);
                swaptions.Add(helper);
            }

            // Set up the optimization problem
            LevenbergMarquardt optimizationMethod = new LevenbergMarquardt(1.0e-8, 1.0e-8, 1.0e-8);
            EndCriteria endCriteria = new EndCriteria(10000, 100, 1e-6, 1e-8, 1e-8);

            //Optimize
            model.calibrate(swaptions, optimizationMethod, endCriteria, new Constraint(), new List<double>());
            EndCriteria.Type ecType = model.endCriteria();

           
            Vector xMinCalculated = model.parameters();
            double yMinCalculated = model.value(xMinCalculated, swaptions);
            Vector xMinExpected = new Vector(2);
        
            double yMinExpected = model.value(xMinExpected, swaptions);

            DVPLI.EstimationResult r = new DVPLI.EstimationResult(new string[] { "Alpha", "Sigma" }, new double[] { xMinCalculated[0],xMinCalculated[1]});
            return r;
        }
        

    }
}
