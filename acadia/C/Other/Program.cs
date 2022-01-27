using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace xjson
{
    class Program
    {
        /// <summary>
        /// Returns the reversed value of the provided JsonToken according to its type (as specified in Assignment C),
        /// or null if the token is not one of: PropertyName, String, Boolean, Number.
        /// </summary>
        /// <param name="token">The JsonToken the value of which is being used for a new reversed value</param>
        /// <param name="value">The value of the JsonToken that is used in making a new reversed value</param>
        /// <returns>The reverse of the given value according to its given JsonToken's type, or null if an invalid TokenType is given.</returns>
        static Tuple<JsonToken, object> reverseJSON(JsonToken token, object value)
        {
            // our program only needs to handle these tokens
            List<JsonToken> validTokens = new List<JsonToken> { JsonToken.PropertyName, JsonToken.String, JsonToken.Float, JsonToken.Integer, JsonToken.Boolean };

            if (validTokens.Contains(token))
            {
                object reversedValue;
                switch (token)
                {
                    case JsonToken.PropertyName:
                        reversedValue = value;
                        break;
                    case JsonToken.String:
                        char[] oldString = ((string)value).ToCharArray(); // cast the value to a string, then make it a character array
                        Array.Reverse(oldString); // using Array class reverse method, reverse the string
                        reversedValue = new string(oldString); // set the reversed value to equal the reversed string
                        break;
                    case JsonToken.Float:
                        reversedValue = -(double)value; // cast the value to a float and change the sign
                        break;
                    case JsonToken.Integer:
                        reversedValue = -(long)value; // cast the value to an int and change the sign
                        break;
                    default: // default is boolean
                        reversedValue = !((bool)value); // cast the value to a bool and then set the reversed value to the negation of that bool
                        break;
                }
                return Tuple.Create(token, reversedValue);
            }
            return null;
        }

        /// <summary>
        /// Given an array of JsonToken, value tuples that have reversed property value pairs, reverse them back such that the properties come first
        /// </summary>
        /// <param name="reversedArray"></param>
        /// <returns></returns>
        static List<Tuple<JsonToken, object>> SwapPropertyValuePairs(List<Tuple<JsonToken, object>> reversedArray)
        {
            // keep track of seen properties to avoid reversing multiple times because the pointer is not moved to past the property value pair that
            // was reversed so that nested propery value pairs also get reversed
            List<string> seenProperties = new List<string>();
            for (int ii = reversedArray.Count - 1; ii > 0; ii--) // loop through given array (backwards to access properties first)
            {
                if (reversedArray[ii].Item1 == JsonToken.PropertyName && !seenProperties.Contains((string)reversedArray[ii].Item2))
                {
                    seenProperties.Add((string)reversedArray[ii].Item2);
                    if (reversedArray[ii - 1].Item1 == JsonToken.EndObject) // if property paired with object
                    {
                        int end = 0;
                        for (int jj = ii - 2; jj >= 0; jj--) // inner loop over nested object
                        {
                            if (reversedArray[jj].Item1 == JsonToken.EndObject) // increment end counter to keep track of nested objects
                            {
                                ++end;

                                // recur on the subobject-------------------------------------------------------------
                                int subEnd = 0;
                                for (int kk = jj - 2; kk >= 0; kk--) // inner loop over nested object
                                {
                                    if (reversedArray[kk].Item1 == JsonToken.EndObject) // increment end counter to keep track of nested objects
                                    {
                                        ++subEnd;
                                    }
                                    else if (reversedArray[kk].Item1 == JsonToken.StartObject && subEnd != 0)
                                    {
                                        --subEnd;
                                    }
                                    else if (reversedArray[kk].Item1 == JsonToken.StartObject && subEnd == 0) // we're at the startObject corresponding to our initial endobject
                                    {
                                        // jj = end object (largest idx)
                                        // kk = start object (kk + 1 is first value within object; jj - kk - 1) gives the amt between start and end objects

                                        // pass the bounds of this object to recur to handle the property value pairs within subobjects
                                        List<Tuple<JsonToken, object>> subObject = new List<Tuple<JsonToken, object>>();
                                        subObject = SwapPropertyValuePairs(reversedArray.GetRange(kk + 1, jj - kk - 1));
                                        reversedArray.RemoveRange(kk + 1, jj - kk - 1);
                                        reversedArray.InsertRange(kk + 1, subObject);
                                        break;
                                    }
                                }
                            }
                            else if (reversedArray[jj].Item1 == JsonToken.StartObject && end != 0)
                            {
                                --end;
                            }
                            else if (reversedArray[jj].Item1 == JsonToken.StartObject && end == 0) // we're at the startObject corresponding to our initial endobject
                            {
                                // ii = property name (largest idx)
                                // jj = end of object (start idx)
                                Tuple<JsonToken, object> property = Tuple.Create(JsonToken.PropertyName, reversedArray[ii].Item2);
                                reversedArray.Insert(jj, property); // copy and move old property before object
                                reversedArray.RemoveAt(ii + 1); // ii + 1 is where the property name is after inserting an item earlier into the list (remove it)
                                break; 
                            }
                        }
                    }
                    else if (reversedArray[ii - 1].Item1 == JsonToken.EndArray) // if property paired with array
                    {
                        int end = 0;
                        for (int jj = ii - 2; jj >= 0; jj--) // inner loop over nested array
                        {
                            if (reversedArray[jj].Item1 == JsonToken.EndArray) // increment end counter to keep track of nested arrays
                            {
                                ++end;
                            }
                            else if (reversedArray[jj].Item1 == JsonToken.StartArray && end != 0)
                            {
                                --end;
                            }
                            else if (reversedArray[jj].Item1 == JsonToken.StartArray && end == 0) // we're at the startArray corresponding to our initial endobject
                            {
                                // ii = property name (largest idx)
                                // jj = end of array (start idx)
                                Tuple<JsonToken, object> property = Tuple.Create(JsonToken.PropertyName, reversedArray[ii].Item2);
                                reversedArray.Insert(jj, property); // copy and move old property before array
                                reversedArray.RemoveAt(ii + 1); // ii + 1 is where the property name is after inserting an item earlier into the list (remove it)
                                //--ii;
                            }
                        }
                    }
                    else if (reversedArray[ii - 1].Item1 != JsonToken.StartObject && reversedArray[ii - 1].Item1 != JsonToken.StartArray) // if property is paired with non-list value
                    {
                        Tuple<JsonToken, object> property = Tuple.Create(JsonToken.PropertyName, reversedArray[ii].Item2);
                        reversedArray.Insert(ii - 1, property); // copy and move old property
                        reversedArray.RemoveAt(ii + 1); // remove old property name
                        //--ii;
                    }
                }
            }
            return reversedArray;
        }

        /// <summary>
        /// swap object bounds given startobject and array bounds given startarray
        /// </summary>
        /// <param name="reversedArray"></param>
        /// <returns></returns>
        static List<Tuple<JsonToken, object>> SwapBounds(List<Tuple<JsonToken, object>> reversedArray, JsonToken jt)
        {
            JsonToken notJt;
            if (jt == JsonToken.StartObject)
            {
                notJt = JsonToken.EndObject;
            }
            else
            {
                notJt = JsonToken.EndArray;
            }

            List<int> startIndices = new List<int>();
            List<int> endIndices = new List<int>();
            for (int ii = 0; ii < reversedArray.Count; ii++)
            {
                if (reversedArray[ii].Item1 == jt)
                {
                    startIndices.Add(ii);
                }
                if (reversedArray[ii].Item1 == notJt)
                {
                    endIndices.Add(ii);
                }
            }
            for (int idx = 0; idx < startIndices.Count; idx++)
            {
                reversedArray[startIndices[idx]] = Tuple.Create(notJt, (object)null);
                reversedArray[endIndices[idx]] = Tuple.Create(jt, (object)null);
            }
            return reversedArray;
        }

        // receives list constituting an object that needs its key value pairs reversed and returns it fixed
        static List<Tuple<JsonToken, object>> ReverseNestedObjectHelper(List<Tuple<JsonToken, object>> obj)
        {
            List<Tuple<JsonToken, object>> rv = new List<Tuple<JsonToken, object>>();

            for (int ii = 0; ii < obj.Count; ii++)
            {
                if (obj[ii].Item1 == JsonToken.PropertyName)
                {
                    int startCount = 0;
                    int endIdx = 0;
                    List<Tuple<JsonToken, object>> tmp = new List<Tuple<JsonToken, object>>();
                    switch (obj[ii + 1].Item1)
                    {
                        case JsonToken.StartArray:
                            for (int jj = ii + 2; jj < obj.Count; jj++)
                            {
                                if (obj[jj].Item1 == JsonToken.StartArray)
                                {
                                    ++startCount;
                                }
                                else if (obj[jj].Item1 == JsonToken.EndArray && startCount != 0)
                                {
                                    --startCount;
                                }
                                else if (obj[jj].Item1 == JsonToken.EndArray && startCount == 0)
                                {
                                    endIdx = jj;
                                    break;
                                }
                            }
                            tmp = ReverseNestedObjects(obj.GetRange(ii + 1, endIdx - ii)); // dont pass bounds
                            rv.InsertRange(0, tmp);
                            rv.Insert(0, obj[ii]);
                            ii = endIdx;
                            break;
                        case JsonToken.StartObject:
                            for (int jj = ii + 2; jj < obj.Count; jj++)
                            {
                                if (obj[jj].Item1 == JsonToken.StartObject)
                                {
                                    ++startCount;
                                }
                                else if (obj[jj].Item1 == JsonToken.EndObject && startCount != 0)
                                {
                                    --startCount;
                                }
                                else if (obj[jj].Item1 == JsonToken.EndObject && startCount == 0)
                                {
                                    endIdx = jj;
                                    break;
                                }
                            }
                            tmp = ReverseNestedObjectHelper(obj.GetRange(ii + 2, endIdx - ii - 2));
                            rv.Insert(0, Tuple.Create(JsonToken.EndObject, (object)null));
                            rv.InsertRange(0, tmp);
                            rv.Insert(0, Tuple.Create(JsonToken.StartObject, (object)null));
                            rv.Insert(0, Tuple.Create(JsonToken.PropertyName, obj[ii].Item2));
                            ii = endIdx;
                            break;
                        default:
                            tmp = obj.GetRange(ii, 2); // get the property key and its singular value
                            rv.InsertRange(0, tmp);
                            break;
                    }
                }
            }
            return rv;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reversedArray"></param>
        /// <param name="initArray"></param> boolean flag that indicates whether the given list is the immediate contents of an array
        /// <returns></returns>
        static List<Tuple<JsonToken, object>> ReverseNestedObjects(List<Tuple<JsonToken, object>> reversedArray)
        {
            for (int ii = 0; ii < reversedArray.Count; ii++)
            {
                if (reversedArray[ii].Item1 == JsonToken.StartObject)
                {
                    int endIdx = 0;
                    int startObjects = 0;
                    for (int jj = ii + 1; jj < reversedArray.Count; jj++)
                    {
                        if (reversedArray[jj].Item1 == JsonToken.StartObject)
                        {
                            ++startObjects;
                        }
                        else if (reversedArray[jj].Item1 == JsonToken.EndObject && startObjects != 0)
                        {
                            --startObjects;
                        }
                        else if (reversedArray[jj].Item1 == JsonToken.EndObject && startObjects == 0)
                        {
                            endIdx = jj;
                            break;
                        }
                    }
                    // fixes this object; pass it a list w object contents (includes bounds)
                    List<Tuple<JsonToken, object>> fixedObjectArray = ReverseNestedObjectHelper(reversedArray.GetRange(ii + 1, endIdx - ii - 1));
                    reversedArray.RemoveRange(ii, endIdx - ii + 1);
                    reversedArray.Insert(ii, Tuple.Create(JsonToken.StartObject, (object)null));
                    reversedArray.InsertRange(ii + 1, fixedObjectArray);
                    reversedArray.Insert(endIdx, Tuple.Create(JsonToken.EndObject, (object)null));
                    ii = endIdx;
                }
            }
            return reversedArray;
        }

        /// <summary>
        /// Given a JsonTextReader that has just read a StartArray JsonToken, return a list of said array's contents, reversed, with each individual
        /// JSON value within the array reversed according to Assignment C specifications, up to but not including the EndArray JsonToken.
        /// </summary>
        /// <param name="reader">The JsonTextReader being iterated over for its array contents that are to be reversed. Pointing to a StartArray JsonToken.</param>
        /// <returns>A list of JsonToken and object tuples corresponding to the reversed contents of the array starting at the given JsonTextReader.</returns>
        static List<Tuple<JsonToken, object>> reverseJSONArray(JsonTextReader reader)
        {
            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new ArgumentException("Given JsonTextReader is not pointing to the beginning of an array.");
            }

            List<Tuple<JsonToken, object>> reversedArray = new List<Tuple<JsonToken, object>>();
            while (reader.Read()) // read in the next JSON token
            {
                if (reader.TokenType == JsonToken.EndArray)
                {
                    break;
                } // reached the end of the array, return the list of reversed items
                if (reader.Value != null) // write the reversed value if it's one of: PropertyName, String, Boolean, Numbers
                {
                    Tuple<JsonToken, object> reversed = reverseJSON(reader.TokenType, reader.Value);
                    if (reversed != null)
                    {
                        reversedArray.Add(reversed); // Write the reversed version of the JSON token
                    }
                }
                else // the current JSON token's value must be one of: StartObject, StartArray, Null, and EndObject
                {
                    reversedArray.Add(Tuple.Create(reader.TokenType, (object)null)); // include the StartObject, StartArray, Null, or EndObject to be
                    if (reader.TokenType == JsonToken.StartArray) // if it's a sub array, need to read until the end of the array, reverse each item accordingly, and then reverse the array's order
                    {
                        int startArrIdx = reversedArray.Count > 0 ? reversedArray.Count - 1 : 0; // store the index at which the StartArray of a subarray is added
                        List<Tuple<JsonToken, object>> reversedSubArray = new List<Tuple<JsonToken, object>>();
                        reversedSubArray.AddRange(reverseJSONArray(reader)); // handle this new array just like we are handling the current array
                        reversedArray.AddRange(reversedSubArray); // add the reversed sub array to the parent array

                        reversedArray.Add(Tuple.Create(JsonToken.EndArray, (object)null)); // add the sub array's EndArray to the parent array
                        reversedArray[startArrIdx] = Tuple.Create(JsonToken.EndArray, (object)null); // swap the Start and EndArrays of the subarray that got reversed to bring them back to their correct order
                        reversedArray[reversedArray.Count - 1] = Tuple.Create(JsonToken.StartArray, (object)null);
                    }

                }
            }
            return reversedArray;
        }

        /// <summary>
        /// Consumes an arbitrarily long series of well-formed JSON from STDIN and delivers an equally long series of JSON to STDOUT.
        /// Each JSON element is reversed according to assignment C specifications before being output.
        /// </summary>
        static void Main()
        {
            StreamReader sr = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(sr);
            StreamWriter sw = new StreamWriter(Console.OpenStandardOutput());
            sw.AutoFlush = true;
            Console.SetOut(sw);

            JsonTextReader reader = new JsonTextReader(sr);
            JsonTextWriter writer = new JsonTextWriter(sw);
            while (reader.Read()) // read in the next JSON token
            {
                if (reader.Value != null) // writes PropertyName, String, Boolean, Numbers
                {
                    Tuple<JsonToken, object> reversed = reverseJSON(reader.TokenType, reader.Value);
                    if (reversed != null)
                    {
                        writer.WriteToken(reversed.Item1, reversed.Item2); // Write the reversed version of the JSON token
                    }
                }
                else
                {
                    writer.WriteToken(reader.TokenType); // writes StartObject, StartArray & EndArray, Null, and EndObject
                    if (reader.TokenType == JsonToken.StartArray) // need to read until the end of the array, reverse each item accordingly, and then reverse the order
                    {
                        List<Tuple<JsonToken, object>> reversedArray = new List<Tuple<JsonToken, object>>();
                        List<Tuple<JsonToken, object>> subarray = reverseJSONArray(reader);
                        reversedArray.AddRange(subarray); // reader now points to the end array
                        reversedArray.Reverse(); // reverse the array
                        reversedArray = SwapBounds(reversedArray, JsonToken.StartObject); // fix the swapped start and end objects
                        reversedArray = SwapPropertyValuePairs(reversedArray); // swap the values coming before the property names
                        reversedArray = ReverseNestedObjects(reversedArray); // objects nested within arrays have been reversed, so rereverse the property value pairs to bring them into the right order
                        reversedArray.ForEach(x => writer.WriteToken(x.Item1, x.Item2));
                        writer.WriteToken(reader.TokenType); // Write the EndArray here because it doesnt add the EndArray anywhere (loops after returning reverseJSONArray, so new Token)
                    }
                }
            }

            reader.Close();
            writer.Close();

            sw.Close();
            sr.Close();
        }
    }
}
