using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


/// <summary>
/// Holds data about the move and the position at which move was made.
/// </summary>
public struct PVEntry
{
    public int move;
    public long hash;
}

public class PVTable
{
    public PVEntry[] data;
    public int numEntries;
}

[StructLayout(LayoutKind.Sequential)]
public class TableEntry
{
    enum ScoreType{
        ACCURATE,
        FAIL_LOW,
        FAIL_HIGH
    }

    // Holds the hash value for this entry
    public long hashValue; // position
    // Holds the type of score stored
    public int scoreType;

    public int minScore;

    public int maxScore;
    // Holds the best move to make (as found on a previous)
    // calculation
    public int bestMove;
    // Holds the depth of calculation at which the score
    // was found
    public int depth;
}

public class Bucket
{
    // The table entry at this location
    TableEntry entry;

    // The next item in the bucket
    Bucket next = null;

    // Returns a matching entry from this bucket, even
    // if it comes further down the list
    public TableEntry GetElement(long hashValue)
    {
        if (entry.hashValue == hashValue)
            return entry;
        if (next != null)
            return next.GetElement(hashValue);
        return null;
    }
}

public class HashTable
{
    // Holds the contents of the table
    public Bucket[] buckets;
    public int Bucket_Size;
    public HashTable(float TableSizeInMB = 5.0f){
        int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(TableEntry));
        Bucket_Size = (int)((TableSizeInMB * (1024f * 1024f)) / size);
        buckets = new Bucket[Bucket_Size];
    }

    // Finds the bucket in which the value in stored
    public Bucket GetBucket(long hashValue)
    {
        return buckets[hashValue % (long)Bucket_Size];
    }
    
    // Retrieves an entry from the table
    public TableEntry GetEntry(long hashValue)
    {
        return GetBucket(hashValue).GetElement(hashValue);
    }
}

public class HashArray
{
    // Holds the entries
    public TableEntry[] entries;
    public int Bucket_Size;
    public HashArray(float TableSizeInMB = 5.0f){
        int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(TableEntry));
        Bucket_Size = (int)((TableSizeInMB * (1024f * 1024f)) / size);
        entries = new TableEntry[Bucket_Size];
    }

    // Retrieves an entry from the table
    public TableEntry GetEntry(long hashValue)
    {
        var entry = entries[hashValue % (long)Bucket_Size];
        if (entry.hashValue == hashValue) return entry;
        else return null;
    }
}

public class TranspositionTable
{
    public TableEntry[] entries;
    public int Bucket_Size;
    public TranspositionTable(float TableSizeInMB = 5.0f){
        int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(TableEntry));
        Bucket_Size = (int)((TableSizeInMB * (1024f * 1024f)) / size);
        entries = new TableEntry[Bucket_Size];
    }

    public TableEntry GetEntry(long hashValue)
    {
        var entry = entries[hashValue % (long)Bucket_Size];
        if (entry != null && entry.hashValue == hashValue) return entry;
        else return null;
    }

    public void StoreEntry(TableEntry entry)
    {
        // Always replace the current entry
        entries[entry.hashValue % (long)Bucket_Size] = entry;
    }
}