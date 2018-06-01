using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvenShittyerCalc
{
    public abstract class NotifyPropertyChangedClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ViewModel : NotifyPropertyChangedClass
    {
        #region Declarations

        private ICommand numberCommand;
        private ICommand operationCommand;

        private StringBuilder result = new StringBuilder("0");
        private StringBuilder history = new StringBuilder();
        private bool editable;
        private double firstNumber;
        private double secondNumber;
        private string lastOperation = "";
        private string aritmeticOperation="";
        private string separator;


        #endregion

        #region Properties

        public string Separator
        {
            get
            {   if(separator==null)
                    separator=CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                return separator;
            }
            set
            {
                separator = value;
                OnPropertyChanged();
            }
        }

        public double FirstNumber
        {
            get
            {
                return firstNumber;
            }
            set
            {
                if (value == firstNumber)
                    return;

                firstNumber = value;
               // OnPropertyChanged();
            }
        }

        public double SecondNumber
        {
            get
            {
                return secondNumber;
            }
            set
            {
                if (value == secondNumber)
                    return;

                secondNumber = value;
                OnPropertyChanged();
            }
        }

       // public double ResultNumber;
        public string Result
        {
            get
            {
                return result.ToString();
            }
            set
            {
                result.Clear();
                result.Append(value);
                if (value.Equals("Cannot divide") || Result.Equals("UnknownBasicOperation"))
                {
                    FirstNumber = 0;
                    SecondNumber = 0;
                    editable = false;
                    aritmeticOperation = "";
                    lastOperation = "";
                }
                OnPropertyChanged("Result");
            }
        }
        public string History
        {
            get
            {
                history.Clear();
                return history.Append(FirstNumber.ToString() + aritmeticOperation.ToString() + SecondNumber.ToString()).ToString();
            }
            set
            {
                history.Clear();
                history.Append(value);
                OnPropertyChanged("History");
            }
        }
        public ICommand NumberCommand
        {
            get
            {
                if (numberCommand == null)
                    numberCommand = new DelegateCommand<string>(NumberClicked);

                return numberCommand;
            }
        }
        public ICommand OperationCommand
        {
            get
            {
                return this.operationCommand
                       ?? (this.operationCommand = new DelegateCommand<string>(OperationClicked));
            }
        }
        #endregion

        #region Methods

        private void OperationClicked(string operation)
        {
            if ("+-*/=".Contains(operation))
            {
                editable = false;
                if (operation == "=")
                {
                    if (aritmeticOperation == "")
                    {
                        return;
                    }
                    else
                    {

                        if (lastOperation != "=")
                        {
                            if (double.TryParse(Result.ToString(), out secondNumber))
                            {
                                Result = BasicCalculations.Calculate(firstNumber, secondNumber, aritmeticOperation).ToString();
                            }
                        }
                        else
                        {
                            Result = BasicCalculations.Calculate(FirstNumber=double.Parse(Result.ToString()), SecondNumber, aritmeticOperation);
                        }
                        //else Result = "Bad input";
                    }

                    //aritmeticOperation = "";
                }
                else
                {
                    if (aritmeticOperation == "")
                    {
                        FirstNumber = double.Parse(Result.ToString());
                    }
                    else
                    {
                        if (lastOperation == "")
                        {
                            if (double.TryParse(Result.ToString(), out secondNumber) == true)
                            {
                                FirstNumber = double.Parse(Result = BasicCalculations.Calculate(FirstNumber, SecondNumber, aritmeticOperation));
                            }
                        }
                        else
                        {
                            if (double.TryParse(Result.ToString(), out firstNumber) == true)
                            {
                                //SecondNumber = double.Parse(Result.ToString()); 
                                  //  Result = BasicCalculations.Calculate(FirstNumber, SecondNumber, aritmeticOperation);
                            }
                        }
                        //else Result = "Bad input";
                    }
                    aritmeticOperation = operation;
                }
                lastOperation = operation;
            }




            if (operation == "f")
            {
                if (Result.StartsWith("-"))
                {
                    Result = Result.Remove(0, 1);
                }
                else
                {
                    Result = "-" + Result;
                }
            }
            if (operation == "b")
            {
                if(!editable)
                {
                    Result = "0";
                    editable = true;
                    return;
                }
                if (Result.Length > 2)
                {
                    Result = Result.Remove(Result.Length - 1, 1);
                }
                else
                {
                    if (Result.StartsWith("-") || Result.Length < 2)
                    {
                        Result = "0";
                    }
                    else
                    {
                        Result = Result.Remove(Result.Length - 1, 1);
                    }
                }
            }
            if (operation == Separator)
            {
                if (!Result.Contains(Separator) && editable)
                {
                    Result = Result + Separator;
                }
            }
            if (operation == "C")
            {
                Result = "0";
                lastOperation = "";
                aritmeticOperation = "";
                FirstNumber = 0;
                SecondNumber = 0;
                editable = true;

            }
            OnPropertyChanged("History");
        }

        private void NumberClicked(string number)
        {

            if(lastOperation=="=")
            {

                aritmeticOperation = "";
                FirstNumber = 0;
                SecondNumber = 0;
            }
            if (!editable)
            {
                Result = "0";

                //aritmeticOperation = "";
                //FirstNumber = 0;
                SecondNumber = 0;
                lastOperation = "";
            }
            if (Result == "0")
            {
                Result = number;
            }
            else Result = Result + number;
            editable = true;
        }
        #endregion
    }
}
