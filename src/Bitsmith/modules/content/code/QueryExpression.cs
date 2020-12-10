using Bitsmith.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitsmith
{
    internal enum OperatorType
    {
        None,
        Equals,
        LessThan,
        GreaterThan,
        LessThanEquals,
        GreaterThanEquals,      
        In,
        InAll,
    }

    public class QueryExpression
    {
        private OperatorType _Operator = OperatorType.None;
        private string[] _KeyValue;
        private string _Query;

        public QueryExpression(string s)
        {
            _Query = s;
            if (s.Contains("<=") | s.Contains(">="))
            {
                _KeyValue = s.Split(new string[] { "<=", ">=" }, StringSplitOptions.RemoveEmptyEntries);
                _Operator = (s.Contains("<=")) ? OperatorType.LessThanEquals : OperatorType.GreaterThanEquals;
            }
            else if (s.Contains(':') | s.Contains('='))
            {
                _KeyValue = s.Split(new char[] { ':', '=' });
                _Query = s.Replace(':', '=');
                _Operator = OperatorType.Equals;
            }
            else if (s.Contains('>') | s.Contains('<'))
            {
                _KeyValue = s.Split(new char[] { '<', '>' });
                _Operator = (s.Contains("<")) ? OperatorType.LessThan : OperatorType.GreaterThan;
            }
        }

        internal bool EvaluateX(List<Property> list)
        {
            bool b = false;
            for (int i = 0; !b && i < list.Count; i++)
            {
                var prop = list[i];
                if (_Operator == OperatorType.None)
                {
                    b = prop.Name.Equals(_Query, StringComparison.OrdinalIgnoreCase);
                }
                else if(_Operator == OperatorType.Equals)
                {

                }
                //string s = list[i].Name.ToLower().Trim();
                //if (s.StartsWith("tds") | s.StartsWith("createdby"))
                //{

                //}
                //else if (!String.IsNullOrEmpty(list[i].Datatype()))
                //{
                //    string t = list[i].Datatype();
                //    switch (t)
                //    {
                //        case "System.Int32":
                //            int x, y;
                //            if (Int32.TryParse(_KeyValue[1], out y) && Int32.TryParse(list[i].Value.ToString(), out x))
                //            {
                //                if (_Operator == OperatorType.LessThan)
                //                {
                //                    b = (x < y);
                //                }
                //                else if (_Operator == OperatorType.LessThanEquals)
                //                {
                //                    b = (x <= y);
                //                }
                //                else if (_Operator == OperatorType.GreaterThan)
                //                {
                //                    b = (x > y);
                //                }
                //                else if (_Operator == OperatorType.GreaterThanEquals)
                //                {
                //                    b = (x >= y);
                //                }
                //            }
                //            break;
                //        case "System.Decimal":
                //            Decimal d;
                //            if (Decimal.TryParse(_KeyValue[1], out d))
                //            {

                //            }
                //            break;
                //        case "System.DateTime":
                //            DateTime dte;
                //            if (DateTime.TryParse(_KeyValue[1], out dte))
                //            {

                //            }
                //            break;
                //        case "System.Boolean":
                //            Boolean bol;
                //            if (_KeyValue[1] == "0")
                //            {

                //            }
                //            else if (_KeyValue[1] == "1")
                //            {

                //            }
                //            else if (Boolean.TryParse(_KeyValue[1], out bol))
                //            {

                //            }
                //            break;
                //        case "System.String":

                //            break;
                //        default:
                //            break;
                //    }

                //    switch (_Operator)
                //    {
                //        case OperatorType.None:
                //            b = true;
                //            break;
                //        case OperatorType.LessThan:
                //            break;
                //        case OperatorType.GreaterThan:
                //            break;
                //        case OperatorType.LessThanEquals:
                //            break;
                //        case OperatorType.GreaterThanEquals:
                //            break;
                //        case OperatorType.Equals:
                //        default:
                //            break;
                //    }
                //}
                //else if (_Operator == OperatorType.Equals)
                //{
                //    s = s.Replace(':', '=');
                //    b = (s == _Query);
                //}
                //else if (_Operator == OperatorType.None)
                //{
                //    if (!String.IsNullOrEmpty(list[i].Datatype()))
                //    {
                //        s = list[i].Name.ToLower();
                //    }
                //    b = (s == _Query);
                //}

                //else if (s.Contains("<=") | s.Contains(">="))
                //{
                //    string[] tag = s.Split(new string[] { "<=", ">=" }, StringSplitOptions.RemoveEmptyEntries);
                //}
                //else if (s.Contains(':') | s.Contains('='))
                //{
                //    // parse for operator
                //    string[] tag = s.Split(new char[] { ':', '=' });
                //    if (list.Contains(s) | list.Contains(tag[0]))
                //    {
                //        b = true;
                //    }
                //}
                //else if (s.Contains('>') | s.Contains('<'))
                //{
                //    string[] tag = s.Split(new char[] { '<', '>' });
                //}
                //else if (list.Contains(s))
                //{
                //    b = true;
                //}
            }

            return b;
        }

        internal bool Evaluate(List<Property> list)
        {
            bool b = false;
            if (_Operator == OperatorType.None)
            {
                for (int i = 0;!b && i < list.Count; i++)
                {
                    var prop = list[i];
                    b = prop.Name.Equals(_Query, StringComparison.OrdinalIgnoreCase);
                }
            }
            else if( _Operator == OperatorType.Equals) // name:value string comparison is adequate
            {
                for (int i = 0; !b && i < list.Count; i++)
                {
                    var prop = list[i];
                    var propval = prop.Value != null ? $"{prop.Name}={prop.Value.ToString()}" : prop.Name;
                    b = propval.Equals(_Query, StringComparison.OrdinalIgnoreCase);
                }
            }
            else if(_Operator == OperatorType.In) // 
            {

            }
            else if(_Operator == OperatorType.InAll)
            {

            }
            else
            {
                for (int i = 0;!b && i < list.Count; i++)
                {
                    var prop = list[i];                    
                    if(prop.Name.Equals(_KeyValue[0], StringComparison.OrdinalIgnoreCase) && prop.Value != null)
                    {
                        var key = prop.Datatype();
                        switch (prop.Datatype())
                        {
                            case "DateTime":
                                break;
                            case "Int32":
                                break;
                            case "Double":
                                break;
                            case "String":
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return b;
        }

        private bool CompareTo<T>(string x, string y, OperatorType operatorType) where T : IConvertible
        {
            bool b = false;

            try
            {
                var xx = Parse<T>(x);
                var yy = Parse<T>(y);
                switch (operatorType)
                {
                    case OperatorType.LessThan:
                        //b = xx < yy;
                        break;
                    case OperatorType.GreaterThan:
                        break;
                    case OperatorType.LessThanEquals:
                        break;
                    case OperatorType.GreaterThanEquals:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return b;
        }
        private static T Parse<T>(string valueToConvert) where T : IConvertible
        {
            return (T)Convert.ChangeType(valueToConvert, typeof(T));
        }
    }
}
