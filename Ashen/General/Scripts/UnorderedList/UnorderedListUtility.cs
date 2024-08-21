using System;
using System.Collections.Generic;

/**
 * This class contains any utilities that are specific for
 * unordered lists, such as the most efficient way to remove
 * from an unorderedList.
 **/
public class UnorderedListUtility<T>
{
    /**
     * As long as order does not matter, the fastest way
     * to remove an element from a list is to swap the
     * last element with the current index and remove at
     * the end. This ensures that the list does not have 
     * to shift all the elements to the right of the removed
     * element down one.
     **/
    public static void RemoveAt(List<T> list, int x)
    {
        if (x >= 0 && x < list.Count)
        {
            T temp = list[x];
            list[x] = list[list.Count - 1];
            list[list.Count - 1] = temp;
            list.RemoveAt(list.Count - 1);
        }
    }
}
