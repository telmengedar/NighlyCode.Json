﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Json.Tests.Data;
using NightlyCode.Json;
using NUnit.Framework;

namespace Json.Tests {
    [TestFixture, Parallelizable]
    public class JsonReaderTests {

        [Test, Parallelizable]
        public void SerializeEnumInObject() {
            string serialized = NightlyCode.Json.Json.WriteString(new TestData {
                WeekDay = DayOfWeek.Wednesday
            });
            TestData deserialized = NightlyCode.Json.Json.Read<TestData>(serialized);
            Assert.AreEqual(DayOfWeek.Wednesday, deserialized.WeekDay);
        }

        [Test, Parallelizable]
        public void SerializeEnumInObjectCamelCased() {
            string serialized = NightlyCode.Json.Json.WriteString(new TestData {
                WeekDay = DayOfWeek.Wednesday
            }, new JsonOptions {
                ExcludeNullProperties = true,
                NamingStrategy = NamingStrategies.CamelCase
            });
            TestData deserialized = NightlyCode.Json.Json.Read<TestData>(serialized);
            Assert.AreEqual(DayOfWeek.Wednesday, deserialized.WeekDay);
        }

        [Test, Parallelizable]
        public void ReadArrayOfArray() {
            object[][] result = NightlyCode.Json.Json.Read<object[][]>("[[1,2],[3,4],[5,6]]");
            Assert.AreEqual(3, result.Length);
            Assert.That(result.All(i => i.Length == 2));
        }
        
        [Test, Parallelizable]
        public void ReadEncodedString() {
            string data = NightlyCode.Json.Json.Read<string>("\"1234567890\"");
            Assert.AreEqual("1234567890", data);
        }
        
        [Test, Parallelizable]
        public void ReadDataMember() {
            SnakeData snakedata = NightlyCode.Json.Json.Read<SnakeData>("{\"over_the_top\": 99}");
            Assert.AreEqual(99, snakedata.OverTheTop);
        }
        
        [Test, Parallelizable]
        public void ReadProperType() {
            object result = NightlyCode.Json.Json.Read("97");
            Assert.AreEqual(97, result);
        }

        [TestCase("Json.Tests.Data.map.json")]
        [Parallelizable]
        public void ReadValidJson(string resource) {
            using Stream datastream = typeof(JsonReaderTests).Assembly.GetManifestResourceStream(resource);
            object result = NightlyCode.Json.Json.Read(datastream);
            Assert.NotNull(result);
        }

        [TestCase("Json.Tests.Data.map.json")]
        [Parallelizable]
        public async Task ReadValidJsonAsync(string resource) {
            using Stream datastream = typeof(JsonReaderTests).Assembly.GetManifestResourceStream(resource);
            object result = await NightlyCode.Json.Json.ReadAsync(datastream);
            Assert.NotNull(result);
        }

        [Test, Parallelizable]
        public void ReadEmptyObject() {
            TestData testdata = NightlyCode.Json.Json.Read<TestData>("{}");
            Assert.NotNull(testdata);
        }

        [Test, Parallelizable]
        public async Task ReadEmptyObjectAsync() {
            TestData testdata = await NightlyCode.Json.Json.ReadAsync<TestData>("{}");
            Assert.NotNull(testdata);
        }

        [Test, Parallelizable]
        public void ReadGuid() {
            Guid guid = NightlyCode.Json.Json.Read<Guid>(NightlyCode.Json.Json.WriteString(Guid.Empty));
            Assert.AreEqual(Guid.Empty, guid);
        }

        [Test, Parallelizable]
        public async Task ReadGuidAsync() {
            Guid guid = await NightlyCode.Json.Json.ReadAsync<Guid>(await NightlyCode.Json.Json.WriteStringAsync(Guid.Empty));
            Assert.AreEqual(Guid.Empty, guid);
        }

        [Test, Parallelizable]
        public void ReadDateTime() {
            DateTime result = NightlyCode.Json.Json.Read<DateTime>("\"2020-12-31T00:00:00.0000000\"");
            Assert.AreEqual(2020, result.Year);
            Assert.AreEqual(12, result.Month);
            Assert.AreEqual(31, result.Day);
        }

        [Test, Parallelizable]
        public void ReadEstTime() {
            DateTime result = NightlyCode.Json.Json.Read<DateTime>("\"2020-06-01 03:08am\"");
            Assert.AreEqual(2020, result.Year);
            Assert.AreEqual(6, result.Month);
            Assert.AreEqual(1, result.Day);
            Assert.AreEqual(3, result.Hour);
            Assert.AreEqual(8, result.Minute);
        }

        [Test, Parallelizable]
        public void ReadEstTimePm() {
            DateTime result = NightlyCode.Json.Json.Read<DateTime>("\"2020-06-01 03:08pm\"");
            Assert.AreEqual(2020, result.Year);
            Assert.AreEqual(6, result.Month);
            Assert.AreEqual(1, result.Day);
            Assert.AreEqual(15, result.Hour);
            Assert.AreEqual(8, result.Minute);
        }

        [Test, Parallelizable]
        public void ReadTimespan() {
            TimeSpan result = NightlyCode.Json.Json.Read<TimeSpan>("\"04:34:22\"");
            Assert.AreEqual(4, result.Hours);
            Assert.AreEqual(34, result.Minutes);
            Assert.AreEqual(22, result.Seconds);
        }

        [Test, Parallelizable]
        public async Task ReadTimespanAsync() {
            TimeSpan result = await NightlyCode.Json.Json.ReadAsync<TimeSpan>("\"04:34:22\"");
            Assert.AreEqual(4, result.Hours);
            Assert.AreEqual(34, result.Minutes);
            Assert.AreEqual(22, result.Seconds);
        }

        [Test, Parallelizable]
        public void ReadGuidProperty() {
            TestData testdata = NightlyCode.Json.Json.Read<TestData>("{\"guid\":\"00000000-0000-0000-0000-000000000000\"}");
            Assert.NotNull(testdata);
            Assert.AreEqual(Guid.Empty, testdata.Guid);
        }
        
        [Test, Parallelizable]
        public async Task ReadGuidPropertyAsync() {
            TestData testdata = await NightlyCode.Json.Json.ReadAsync<TestData>("{\"guid\":\"00000000-0000-0000-0000-000000000000\"}");
            Assert.NotNull(testdata);
            Assert.AreEqual(Guid.Empty, testdata.Guid);
        }

        [TestCase("Json.Tests.Data.testarray.json")]
        [TestCase("Json.Tests.Data.emptyobjectproperty.json")]
        [TestCase("Json.Tests.Data.emptyobjectpropertyinarray.json")]
        [TestCase("Json.Tests.Data.campaign.json")]
        [Parallelizable]
        public void ReadData(string resource) {
            object result = NightlyCode.Json.Json.Read(typeof(JsonReaderTests).Assembly.GetManifestResourceStream(resource));
        }
        
        [TestCase("Json.Tests.Data.testarray.json")]
        [TestCase("Json.Tests.Data.emptyobjectproperty.json")]
        [TestCase("Json.Tests.Data.emptyobjectpropertyinarray.json")]
        [TestCase("Json.Tests.Data.campaign.json")]
        [Parallelizable]
        public async Task ReadDataAsync(string resource) {
            await NightlyCode.Json.Json.ReadAsync(typeof(JsonReaderTests).Assembly.GetManifestResourceStream(resource));
        }

        [TestCase("Json.Tests.Data.testarray.json")]
        [TestCase("Json.Tests.Data.emptyobjectproperty.json")]
        [TestCase("Json.Tests.Data.emptyobjectpropertyinarray.json")]
        [TestCase("Json.Tests.Data.campaign.json")]
        [Parallelizable]
        public async Task ReadAsyncFromAutoClosingStream(string resource) {
            await using AutoclosingStream acl = new AutoclosingStream(typeof(JsonReaderTests).Assembly.GetManifestResourceStream(resource)); 
            await NightlyCode.Json.Json.ReadAsync(acl);
        }
        
        [Test, Parallelizable]
        public void ReadAsDictionaryInterface() {
            NightlyCode.Json.Json.Read<IDictionary<string, object>>("{\"name\":\"Günther\"}");
        }

        [Test, Parallelizable]
        public void ReadTypeFromStructure() {
            object structure = NightlyCode.Json.Json.Read<object>("{\"long\":4,\"childTestData\":{\"string\": \"abc\"},\"array\":[4,3,3,9], \"objectArray\":[{\"string\": \"abc1\"},{\"string\": \"abc2\"}]}");
            TestData data = NightlyCode.Json.Json.Read<TestData>(structure);
            Assert.NotNull(data);
            Assert.NotNull(data.ChildTestData);
            Assert.NotNull(data.Array);
            Assert.NotNull(data.ObjectArray);
            Assert.AreEqual(4, data.Long);
            Assert.AreEqual("abc", data.ChildTestData.String);
            Assert.That(new int[] { 4, 3, 3, 9 }.SequenceEqual(data.Array));
            Assert.AreEqual(2, data.ObjectArray.Length);
            Assert.AreEqual("abc1", data.ObjectArray[0].String);
            Assert.AreEqual("abc2", data.ObjectArray[1].String);
        }
        
        [Test, Parallelizable]
        public void ReadArrayFromStructure() {
            object structure = NightlyCode.Json.Json.Read<object>("[{\"string\": \"abc1\"},{\"string\": \"abc2\"}]");
            TestData[] data = NightlyCode.Json.Json.Read<TestData[]>(structure);
            Assert.NotNull(data);
            Assert.AreEqual(2, data.Length);
            Assert.AreEqual("abc1", data[0].String);
            Assert.AreEqual("abc2", data[1].String);
        }

        [Test, Parallelizable]
        public void ReadDictionaryInterface() {
            object structure = NightlyCode.Json.Json.Read<object>("{\"dictionary\": {\"name\": 8}}");
            DataWithDictionary dictionary = NightlyCode.Json.Json.Read<DataWithDictionary>(structure);
            Assert.NotNull(dictionary);
            Assert.NotNull(dictionary.Dictionary);
            Assert.AreEqual(8L, dictionary.Dictionary["name"]);
        }
    }
}