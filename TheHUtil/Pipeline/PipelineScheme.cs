namespace TheHUtil.Pipeline
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using TheHUtil.Extensions;
    using TheHUtil.IOHelper;

    /// <summary>
    /// Defines the <see cref="PipelineScheme"/> class.
    /// </summary>
    [Serializable]
    public class PipelineScheme: IEnumerable<List<StepMetadata>>
    {
        /// <summary>
        /// The indexed collection of steps that are defined in this <see cref="PipelineScheme"/> instance.
        /// </summary>
        private Dictionary<Type, List<StepMetadata>> metadata;

        /// <summary>
        /// Gets the number of steps defined in this configuration.
        /// </summary>
        public int Count
        {
            get
            {
                var total = 0;
                foreach (var metaCollection in this.metadata.Values)
                {
                    total = total + metaCollection.Count;
                }

                return total;
            }
        }

        /// <summary>
        /// Gets whether this configuration has any steps defined.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.metadata.Count == 0;
            }
        }
                
        /// <summary>
        /// Initializes the static instance of the <see cref="PipelineScheme"/> class. This simply creates the parser for creating configurations.
        /// </summary>
        static PipelineScheme()
        {
            // should create a KingISS pipeline that is dedicated to parsing the strings
            // it should return a collection of "StepMetadata"s that is simply inserted into the scheme.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineScheme"/> class.
        /// </summary>
        public PipelineScheme()
        {
            this.metadata = new Dictionary<Type, List<StepMetadata>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineScheme"/> class using the provided data.
        /// </summary>
        /// <param name="metadataCollection">The collection of metadata to copy.</param>
        public PipelineScheme(IEnumerable<StepMetadata> metadataCollection) :
            this()
        {
            foreach (var meta in metadataCollection)
            {
                if (!this.metadata.ContainsKey(meta.TypeContainingMethod))
                {
                    this.TryAdd(meta);
                }
                else
                {
                    List<StepMetadata> stepMetas;
                    this.metadata.TryGetValue(meta.TypeContainingMethod, out stepMetas);
                    stepMetas.Add(meta);
                }
            }
        }

        /// <summary>
        /// Loads a pipeline configuration, or scheme, from a given file.
        /// </summary>
        /// <param name="fileName">The name of the file, optionally with the path, of the file to retrieve the data from.</param>
        /// <returns>A new <see cref="PipelineScheme"/> instance containing the data defined in the given file.</returns>
        public static PipelineScheme Load(string fileName)
        {
            return PipelineScheme.Parse(IOHelper.GetFileStream(fileName, FileMode.Open));
        }

        /// <summary>
        /// Extracts the configuration data for a pipeline from a string.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        /// <returns>An instance of the <see cref="PipelineScheme"/> class that contains the configuration defined in the string.</returns>
        public static PipelineScheme Parse(string data)
        {
            var lines = data.Split(new[] { "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var metas = new List<StepMetadata>(lines.Length);
            for (var index = 0; index < lines.Length; index++)
            {
                StepMetadata meta;
                if (StepMetadata.TryParse(lines[index], out meta))
                {
                    metas.Add(meta);
                }
            }

            metas.TrimExcess();
            var result = new PipelineScheme(metas);
            return result;
            // TODO: create a KingISS version of this parser.
        }

        /// <summary>
        /// Extracts the configuration data for a pipeline from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="schemeStream">The <see cref="Stream"/> instance to extract the data from.</param>
        /// <returns>An instance of the <see cref="PipelineScheme"/> class that contains the configuration defined in the <see cref="Stream"/>.</returns>
        public static PipelineScheme Parse(Stream schemeStream)
        {
            // Should be replaced with something that can handle smaller memory confines
            // IDEA: try reading it line for line.
            // would require refactoring a portion of the Parse(string) method.
            using (var reader = new StreamReader(schemeStream))
            {
                return PipelineScheme.Parse(reader.ReadToEnd());
            }            
        }

        /// <summary>
        /// Attempts to load a configuration from a file.
        /// </summary>
        /// <param name="fileName">The name of a file, optionally with the path, to retrieve the data from.</param>
        /// <param name="scheme">If true: a <see cref="PipelineScheme"/> instance that represents the data contain in the desired file. If false: this will be null.</param>
        /// <returns>True: only if the loading was successful.</returns>
        public static bool TryLoad(string fileName, out PipelineScheme scheme)
        {
            try
            {
                scheme = PipelineScheme.Load(fileName);
                return true;
            }
            catch
            {
                scheme = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to parse a configuration from a given string.
        /// </summary>
        /// <param name="data">The string to parse.</param>
        /// <param name="scheme">If true: a <see cref="PipelineScheme"/> instance that represents the data contain in the desired file. If false: this will be null.</param>
        /// <returns>True: only if the loading was successful.</returns>
        public static bool TryParse(string data, out PipelineScheme scheme)
        {
            try
            {
                scheme = PipelineScheme.Parse(data);
                return true;
            }
            catch
            {
                scheme = null;
                return false;
            }
        }

        /// <summary>
        /// Attempts to parse a configuration from a given <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="schemeStream">A <see cref="Stream"/> instance that contains the necessary data.</param>
        /// <param name="scheme">If true: a <see cref="PipelineScheme"/> instance that represents the data contain in the desired file. If false: this will be null.</param>
        /// <returns>True: only if the loading was successful.</returns>
        public static bool TryParse(Stream schemeStream, out PipelineScheme scheme)
        {
            try
            {
                scheme = PipelineScheme.Parse(schemeStream);
                return true;
            }
            catch
            {
                scheme = null;
                return false;
            }
        }

        /// <summary>
        /// Copies all of the data from this <see cref="PipelineScheme"/> instance into a new instance.
        /// </summary>
        /// <returns>A new instance of the <see cref="PipelineScheme"/> containing the same data as this instance.</returns>
        public PipelineScheme Clone()
        {
            var result = new PipelineScheme()
            {
                metadata = new Dictionary<Type, List<StepMetadata>>(this.metadata)
            };

            return result;
        }

        /// <summary>
        /// Returns an enumerator that iterates through all the metadatas that have been entered.
        /// </summary>
        /// <returns>The enumerator for all the stored metadatas.</returns>
        public IEnumerator<List<StepMetadata>> GetEnumerator()
        {
            return this.metadata.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through all the metadatas that have been entered.
        /// </summary>
        /// <returns>The enumerator for all the stored metadatas.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.metadata.Values.GetEnumerator();
        }

        /// <summary>
        /// Attempts to add a specific metadata to the configuration schematic.
        /// </summary>
        /// <param name="metadata">The metadata to add.</param>
        /// <returns>False only if the metadata already exists in the configuration schematic.</returns>
        public bool TryAdd(StepMetadata metadata)
        {
            if (this.metadata.ContainsKey(metadata.TypeContainingMethod))
            {
                if (this.metadata[metadata.TypeContainingMethod].Contains(metadata))
                {
                    return false;
                }
                else
                {
                    this.metadata[metadata.TypeContainingMethod].Add(metadata);
                    return true;
                }
            }
            else
            {
                this.metadata.Add(metadata.TypeContainingMethod, new List<StepMetadata>() { metadata });
                return true;
            }
        }

        /// <summary>
        /// Attempts to add a specific metadata, represented by a string, to the configuration schematic.
        /// </summary>
        /// <param name="metadata">The metadata to add.</param>
        /// <returns>False only if the metadata already exists in the configuration schematic.</returns>
        public bool TryAdd(string metadataString)
        {
            StepMetadata meta;
            if (!StepMetadata.TryParse(metadataString, out meta))
            {
                return false;
            }
            else
            {
                return this.TryAdd(meta);
            }
        }

        /// <summary>
        /// Attempts to add a collection of metadatas into the configuration schematic.
        /// </summary>
        /// <param name="key">The type of object the methods come from.</param>
        /// <param name="metadataCollection">The collection of metadatas to add.</param>
        /// <returns>False only if the metadatas already exist in the schematic.</returns>
        public bool TryAdd(Type key, IEnumerable<StepMetadata> metadataCollection)
        {
            if (this.metadata.ContainsKey(key))
            {
                bool result = false;
                if (metadataCollection is IList<StepMetadata>)
                {
                    var metaList = metadataCollection as IList<StepMetadata>;
                    for (var index = 0; index < metaList.Count; index++)
                    {
                        if (!this.metadata[key].Contains(metaList[index]))
                        {
                            result = true;
                            this.metadata[key].Add(metaList[index]);
                        }
                    }
                }
                else
                {
                    foreach (var meta in metadataCollection)
                    {
                        if (!this.metadata[key].Contains(meta))
                        {
                            result = true;
                            this.metadata[key].Add(new StepMetadata(meta.MethodName, meta.TypeContainingMethod, meta.InputType, meta.OutputType, meta.Location.ToString()));
                        }
                    }
                }

                return result;
            }
            else
            {
                this.metadata.Add(key, new List<StepMetadata>(metadataCollection));
                return true;
            }
        }

        /// <summary>
        /// Attempts to retrieve a collection of metadatas that all share the same type that the method is to come from.
        /// </summary>
        /// <param name="key">The type of object that contains the method to create a delegate of.</param>
        /// <param name="value">The collection of metadatas that all share the same type that the methods are to come from.</param>
        /// <returns>True only if the collection exists and has been retrieved.</returns>
        public bool TryGet(Type key, out List<StepMetadata> value)
        {
            return this.metadata.TryGetValue(key, out value);
        }

        /// <summary>
        /// Attempts to get and remove the metadata collection as specified by the type key.
        /// </summary>
        /// <param name="key">The type of object that the metadata's method is declared in.</param>
        /// <param name="value">The collection of metadatas that the key refers to. This has also been removed from the configuration schematic.</param>
        /// <returns>True if the key exists and has been removed. False if the key does not exist.</returns>
        public bool TryRemove(Type key, out List<StepMetadata> value)
        {
            var getResult = this.TryGet(key, out value);
            var removeResult = this.metadata.Remove(key);
            return getResult && removeResult;
        }
    }
}
