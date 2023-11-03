using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    [SerializeField]
    private List<Tag> tags;

    public List<Tag> All => tags;

    public bool HasTag(Tag t)
    {
        return tags.Contains(t);
    }

    public bool HasTag(string tagName)
    {
        return tags.Exists(t => t.Name.Equals(tagName, System.StringComparison.InvariantCultureIgnoreCase));
    }
}
