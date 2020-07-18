using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace MetadataTool
{
    internal class TestMetadata
    {
        public Status status { get; set; }
        public TestFrameWork test_framework { get; set; }

        public List<string> test_dependencies { get; set; }
        public string title { get; set; }

        public string last_mod_by { get; set; }

        public string primary_owner_entity_type { get; set; }

        public string file_url { get; set; }

        public int use_resource_ops_libs { get; set; }

        public int test_tier { get; set; }
        public string name { get; set; }

        public string version { get; set; }

        public TestClassification test_classification { get; set; }

        public string purpose { get; set; }

        public string timeout { get; set; }

        public string primary_owner_entity_name { get; set; }

        public int is_manual { get; set; }

        public DateTime last_mod_date { get; set; }

        public List<Dictionary<string, object>> resources { get; set; }
    }

    internal class TestCaseMetadata
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("testcase_ref_id")]
        public int TestCaseId { get; set; }

        [BsonElement("metadata")]
        public TestMetadata Metadata { get; set; }
    }

    internal class TestDiscovery
    {
        public string repo_branch { get; set; }

        public string datatype { get; set; }

        public string repo_type { get; set; }

        public string repo_url { get; set; }
        
        public List<TestMetadata> data { get; set; }

    }

    internal enum Status
    {
        Development,
        Stable,
        Repair
    }

    internal enum TestClassification
    {
        Unit,
        Package,
        System,
        Duration,
        Memory
    }

    internal enum TestFrameWork
    {
        CppUnit,
        Sal,
        Sas,
        Tem,
        Robot,
        Alm,
        TestRail,
        Ctf,
        PeTest,
        Stf
    }
}