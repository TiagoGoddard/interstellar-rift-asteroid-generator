using AsteroidFieldGenerator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsteroidFieldGenerator.Utility
{
    class AsteroidResourceJsonConverter : JsonConverter<AsteroidResource<Resource>>
    {

        public override bool CanConvert(Type typeToConvert) => typeof(AsteroidResource<Resource>).IsAssignableFrom(typeToConvert);

        public override AsteroidResource<Resource> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = reader.GetString();
            Resource type;
            try
            {
                type = (CommonResource) Enum.Parse(typeof(ResourceType.CommonResource), propertyName);
            } catch {
                try
                {
                    type = (RareResource) Enum.Parse(typeof(ResourceType.RareResource), propertyName);
                } catch {
                    throw new JsonException();
                }
            }

            if(type == null)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            AsteroidResource<Resource> asteroid = new AsteroidResource<Resource>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return person;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "CreditLimit":
                            decimal creditLimit = reader.GetDecimal();
                            ((Customer)person).CreditLimit = creditLimit;
                            break;
                        case "OfficeNumber":
                            string officeNumber = reader.GetString();
                            ((Employee)person).OfficeNumber = officeNumber;
                            break;
                        case "Name":
                            string name = reader.GetString();
                            person.Name = name;
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, AsteroidResource<Resource> asteroidField, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (person is Customer customer)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Customer);
                writer.WriteNumber("CreditLimit", customer.CreditLimit);
            }
            else if (person is Employee employee)
            {
                writer.WriteNumber("TypeDiscriminator", (int)TypeDiscriminator.Employee);
                writer.WriteString("OfficeNumber", employee.OfficeNumber);
            }

            writer.WriteString("Name", person.Name);

            writer.WriteEndObject();
        }
    }
}