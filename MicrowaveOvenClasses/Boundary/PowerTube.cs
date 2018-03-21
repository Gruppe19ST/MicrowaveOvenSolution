using System;
using MicrowaveOvenClasses.Interfaces;

namespace MicrowaveOvenClasses.Boundary
{
    public class PowerTube : IPowerTube
    {
        private IOutput myOutput;
        private int _maxPower = 700;
        private double _percent;
        private int _percentage;

        private bool IsOn = false;

        public PowerTube(IOutput output)
        {
            myOutput = output;
        }

        public int PowerPercentage(int power)
        {
            _percent = (Convert.ToDouble(power) / _maxPower) * 100;
            _percentage = Convert.ToInt32(_percent);

            return _percentage;
        }

        public void TurnOn(int power)
        {
            _percentage = PowerPercentage(power);

            // _percentage indsat i stedet for power
            if (_percent < 1 || 100 < _percent)
            {
                throw new ArgumentOutOfRangeException("percent", _percentage, "Must be between 1 and 100 % (incl.)");
            }

            if (IsOn)
            {
                throw new ApplicationException("PowerTube.TurnOn: is already on");
            }

            myOutput.OutputLine($"PowerTube works with {_percentage} %");
            IsOn = true;
        }

        public void TurnOff()
        {
            if (IsOn)
            {
                myOutput.OutputLine($"PowerTube turned off");
            }

            IsOn = false;
        }
    }
}