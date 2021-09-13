namespace SpecFlowProject1.Services
{
    public class CalculatorService
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }

        public int Add() => Number1 + Number2;
    }
}
