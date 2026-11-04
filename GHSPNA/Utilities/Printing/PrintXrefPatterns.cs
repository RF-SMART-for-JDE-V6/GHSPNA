using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ICS.Stoneridge.Wireless.RFS.Database.TableObjects;

namespace ICS.Stoneridge.Wireless.JDE.Utilities.Printing
{
    /// <summary>
    /// Used to manage the Print Xref search patterns used for retrieving a valid RFS_PrintXref record
    /// </summary>
    public class PrintXrefPatterns : IEnumerable<KeyValuePair<int, PrintXrefType>>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new PrintXrefPatterns
        /// </summary>
        public PrintXrefPatterns()
        {
            Patterns = new SortedDictionary<int, PrintXrefType>();
        }

        #endregion

        #region Properties

        #region Patterns

        /// <summary>
        /// Get the collection of Print Xref search patterns
        /// </summary>
        protected SortedDictionary<int, PrintXrefType> Patterns { get; private set; }

        #endregion

        #endregion

        #region Reset

        /// <summary>
        /// Resets the Print Xref search pattern collection back to its default state
        /// </summary>
        public virtual void Reset()
        {
            Patterns.Clear();

            Patterns.Add(0, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                             PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item | PrintXrefType.Location |
                             PrintXrefType.MixedContainer));
            Patterns.Add(1, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                             PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item | PrintXrefType.Location));
            Patterns.Add(2, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                             PrintXrefType.ContainerType | PrintXrefType.Customer | PrintXrefType.Item));
            Patterns.Add(3, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                             PrintXrefType.ContainerType | PrintXrefType.Customer));
            Patterns.Add(4, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch |
                             PrintXrefType.ContainerType));
            Patterns.Add(5, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version | PrintXrefType.Branch));
            Patterns.Add(6, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.Version));
            Patterns.Add(7, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID | PrintXrefType.DefaultVersion));
            Patterns.Add(8, (PrintXrefType.EnvironmentID | PrintXrefType.WorkflowID));
        }

        #endregion

        #region Add

        /// <summary>
        /// Adds a new Print Xref search pattern to the end of the collection
        /// </summary>
        /// <param name="pattern">Print Xref Search Pattern</param>
        public void Add(PrintXrefType pattern)
        {
            if (Patterns.Count > 0)
            {
                Patterns.Add((Patterns.Last().Key + 1), pattern);
            }
            else
            {
                Patterns.Add(0, pattern);
            }
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new Print Xref search pattern into the collection at the specified stage
        /// </summary>
        /// <param name="stage">Stage</param>
        /// <param name="pattern">Print Xref Search Pattern</param>
        public void Insert(int stage, PrintXrefType pattern)
        {
            //If the collection already contains the stage, we'll need to shift the keys to make room for the
            //new pattern being inserted
            if (Patterns.ContainsKey(stage))
            {
                SortedDictionary<int, PrintXrefType> temp = new SortedDictionary<int, PrintXrefType>();
                foreach (KeyValuePair<int, PrintXrefType> item in Patterns)
                {
                    if (item.Key < stage)
                    {
                        temp.Add(item.Key, item.Value);
                    }
                    else //if equal to or greater than the current stage, increment by one
                    {
                        temp.Add((item.Key + 1), item.Value);
                    }
                }

                //Once we have opened up the index to insert the new pattern, the collection is copied back over
                Patterns.Clear();
                Patterns = new SortedDictionary<int, PrintXrefType>(temp);
            }

            Patterns.Add(stage, pattern);
        }

        #endregion

        #region Clear

        /// <summary>
        /// Removes all the Print Xref search patterns from the collection 
        /// </summary>
        public void Clear()
        {
            Patterns.Clear();
        }

        #endregion

        #region GetEnumerator

        /// <summary>
        /// Returns an enumerator that iterates through the collection
        /// </summary>
        public IEnumerator<KeyValuePair<int, PrintXrefType>> GetEnumerator()
        {
            return Patterns.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Patterns).GetEnumerator();
        }

        #endregion
    }
}