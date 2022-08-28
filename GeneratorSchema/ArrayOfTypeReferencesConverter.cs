﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeneratorLib
{
    public class ArrayOfTypeReferencesConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var singleItem = ReadItem(reader);
            if (singleItem != null)
            {
                return new[] { singleItem };
            }

            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new ArgumentException("Unexpected token type " + reader.TokenType);
            }

            var tokens = new List<TypeReference>();
            reader.Read();
            while (reader.TokenType != JsonToken.EndArray)
            {
                singleItem = ReadItem(reader);
                if (singleItem != null)
                {
                    tokens.Add(singleItem);
                    reader.Read();
                    continue;
                }

                throw new ArgumentException("Unexpected token type " + reader.TokenType);
            }

            return tokens.ToArray();
        }

        private TypeReference ReadItem(JsonReader reader)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new TypeReference { IsReference = false, Name = (string)reader.Value };
            }

            if (reader.TokenType == JsonToken.StartObject)
            {
                return ReadObject(reader);
            }

            return null;
        }

        private TypeReference ReadObject(JsonReader reader)
        {
            reader.Read();
            if (reader.TokenType != JsonToken.PropertyName || ((string)reader.Value != "__ref__" && (string)reader.Value != "type"))
            {
                throw new ArgumentException("Unexpected token type " + reader.TokenType);
            }

            var isRef = (string)reader.Value == "__ref__";

            reader.Read();
            if (reader.TokenType != JsonToken.String)
            {
                throw new ArgumentException("Unexpected token type " + reader.TokenType);
            }

            var retval = new TypeReference
            {
                IsReference = isRef,
                Name = (string)reader.Value
            };
            reader.Read();

            return retval;
        }

        public override bool CanConvert(Type type)
        {
            return true;
        }

        public override void WriteJson(JsonWriter w, object o, JsonSerializer s)
        {
            throw new NotImplementedException();
        }
    }
}
