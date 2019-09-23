/*
 * Name: Angela Robinson
 * Class: CSC382 - Data Structures and Algorithms
 * Date: 9/20/2019
 * Description:
 * Implement a templated Binary Search Tree data structure, implementing the following functionality:
 * -Insert (insert the specified value into the BST)
 * -Delete (delete given node, restructure the tree accordingly so no other data is removed or lost)
 * -Maximum (Find maximum value in the BST)
 * -Traverse (Traverse the tree and return the data in the order of traversal. You can implement either inorder, preorder, or postorder traversal.)
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ARBinarySearchTree
{
    /// <summary>
    /// Generic Binary search tree node where the node only has two children properties and one parent property in addition to the element data.
    /// This class inherits from the Camparer generic class
    /// </summary>
    /// <typeparam name="T">data type that the node can be</typeparam>
    public class BSTNode<T> : Comparer<T>
    {
        /// <summary>
        /// element data information
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// property that holds the parent node
        /// </summary>
        public BSTNode<T> Parent { get; set; }

        /// <summary>
        /// property that holds the right child (greater than) node
        /// </summary>
        public BSTNode<T> RtChild { get; set; }

        /// <summary>
        /// property that holds the left child (Less than) node
        /// </summary>
        public BSTNode<T> LftChild { get; set; }

        /// <summary>
        /// Default constructor that creates a null node
        /// </summary>
        public BSTNode() : this(default) { }

        /// <summary>
        /// Constructor that takes only one parameter, the element data
        /// </summary>
        /// <param name="info">the element data</param>        
        public BSTNode(T info) : this(info, null, null, null) { }

        /// <summary>
        /// Constructor that take two parameters, the element data and the parent node
        /// </summary>
        /// <param name="info">the element data</param>
        /// <param name="prnt">the BSTNode that is to be the parent of this node</param>
        public BSTNode(T info, BSTNode<T> prnt) : this(info, prnt, null, null) { }

        /// <summary>
        /// Constructor that takes 4 parameters, the element data, parent node, right node, and left node
        /// </summary>
        /// <param name="info">the element data</param>
        /// <param name="prnt"></param>
        /// <param name="rt"></param>
        /// <param name="lft"></param>
        public BSTNode(T info, BSTNode<T> prnt, BSTNode<T> rt, BSTNode<T> lft)
        {
            Data = info;
            Parent = prnt;
            RtChild = rt;
            LftChild = lft;
        }
        
        /// <summary>
        /// Must implement inherited method 
        /// because BSTNode inherits the Comparer generic class
        /// this is done in order to allow for traversing through a Binary Search Tree
        /// Returns less than 0 if x less than y
        /// Returns 0 if x equals y
        /// Returns greater than 0 if x is greater than y
        /// </summary>
        /// <param name="x">first node data to compare</param>
        /// <param name="y">second node data to compare</param>
        /// <returns></returns>
        public override int Compare(T x, T y) 
        {
            if (x is IComparable<T>)
            {
                return ((IComparable<T>)x).CompareTo(y);
            }
            else
            {                
                return StringComparer.CurrentCulture.Compare(x, y);
            }
        }

        /// <summary>
        /// Calls the overridden inherited Compare method
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int CompareTo(T x)
        {
            return Compare(this.Data, x);
        }
    }


    /// <summary>
    /// Generic Binary Search Tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BST<T> : /*Comparer<BSTNode<T>>,*/ ICollection<BSTNode<T>>
    {
        /// <summary>
        /// Gets and sets the first node of the tree
        /// </summary>
        public BSTNode<T> Root { get; set; }

        /// <summary>
        /// keeps track of the number of nodes in the tree
        /// NOTE: Must have this property due to implementing ICollection
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// BST is not read only
        /// NOTE: Must have this property due to implementing ICollection
        /// </summary>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// Adds a new node to the binary search tree using only the data you want held in the node
        /// </summary>
        /// <param name="itemData">element data you want to be held in the BSTNode</param>
        public void Add(T itemData)
        {
            //create new node with provided element data
            BSTNode<T> newNode = new BSTNode<T>(itemData);

            //add the newly created node to the tree
            Add(newNode);
        }

        /// <summary>
        /// Adds a new node to the binary search tree
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        /// <param name="item">the BSTNode to add to the tree</param>
        public void Add(BSTNode<T> item)
        {
            

            if (Root == null)
            {
                //if root is empty make item the root node
                Root = item;
                Count++;
                return;
            }

            //make temp variable to hold place in tree
            BSTNode<T> current = Root;

            //while current is not null go through the tree
            while (current != null)
            {
                //temp result of compare so that we don't have to continue comparing within each if statement
                int compResult = item.CompareTo(current.Data);

                //if root is not empty compare to see if the new item is less than or greater than
                if (compResult == 0)
                {
                    //if equal do nothing because we don't want duplicates
                    throw new Exception("Duplicate node: " +item.Data +  ".  We don't allow that");
                    //return;
                }
                else if (compResult < 0)
                {
                    if (current.LftChild == null)
                    {
                        //if left child is empty add item to that node
                        current.LftChild = item;
                        item.Parent = current;
                        current = null;
                    }
                    else
                    {
                        //change current to the left child of current                        
                        current = current.LftChild;
                    }
                }
                else
                {
                    if (current.RtChild == null)
                    {
                        //if left child is empty add item to that node
                        current.RtChild = item;
                        item.Parent = current;
                        current = null;
                    }
                    else
                    {
                        //change current to the left child of current                        
                        current = current.RtChild;
                    }
                }
            }
            
            //add to count
            Count++;
            
        }

        /// <summary>
        /// Removes element data out of binary search tree
        /// </summary>
        /// <param name="itemData">element data you want to remove from the tree</param>
        /// <returns></returns>
        public bool Remove(T itemData)
        {
            //create new node with provided element data
            BSTNode<T> newNode = new BSTNode<T>(itemData);

            //add the newly created node to the tree
            return Remove(newNode);
        }

        /// <summary>
        /// Removes node from the binary search tree
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(BSTNode<T> item)
        {
            if (Root == null)
            {
                throw new NullReferenceException("This is an empty tree.");
            }
            //make temp variable to hold place in tree
            BSTNode<T> current = Root;

            //make temp variable to hold the found node to be deleted
            BSTNode<T> toDel;


            //while current is not null go through the tree
            while (current != null)
            {
                //temp result of compare so that we don't have to continue comparing within each if statement
                int compResult = item.CompareTo(current.Data);

                //if root is not empty compare to see if the new item is less than or greater than
                if (compResult == 0)
                {
                    //if equal then we found the node to be deleted
                    //remove one from count
                    Count--;

                    //save a copy of the node to delete
                    toDel = current;

                    //if the node to delete doesn't have children, then delete                    
                    if (toDel.LftChild == null && toDel.RtChild == null)
                    {
                        //null out the reference to the node to be deleted on the parent
                        if (toDel.Parent.LftChild.Equals(toDel))
                        {
                            toDel.Parent.LftChild = null;
                        }
                        else if (toDel.Parent.RtChild.Equals(toDel))
                        {
                            toDel.Parent.RtChild = null;
                        }

                        //make sure parent is null
                        if (toDel.Parent != null)
                        {
                            toDel.Parent = null;
                        }
                    }
                    else if (toDel.LftChild != null && toDel.RtChild == null)
                    {
                        //if there isn't a right child on the node to delete, 
                        //but there is a left child then we make that left childs parent the same 
                        //as the one that is being deleted
                        toDel.LftChild.Parent = toDel.Parent;

                        //change out the reference to the node to be deleted on the parent
                        if (toDel.Parent.LftChild.Equals(toDel))
                        {
                            toDel.Parent.LftChild = toDel.LftChild;
                        }
                        else if (toDel.Parent.RtChild.Equals(toDel))
                        {
                            toDel.Parent.RtChild = toDel.LftChild;
                        }

                        //after reassigning the parent to the new replacement node,
                        //we need to null out the parent of the node to delete, 
                        //as well as the left child reference
                        toDel.Parent = null;
                        toDel.LftChild = null;
                    }
                    else if (toDel.RtChild != null && toDel.LftChild == null)
                    {
                        //if there isn't a left child on the node to delete, 
                        //but there is a right child then we make that right childs parent the same 
                        //as the one that is being deleted
                        toDel.RtChild.Parent = toDel.Parent;

                        //change out the reference to the node to be deleted on the parent
                        if (toDel.Parent.LftChild.Equals(toDel))
                        {
                            toDel.Parent.LftChild = toDel.RtChild;
                        }
                        else if (toDel.Parent.RtChild.Equals(toDel))
                        {
                            toDel.Parent.RtChild = toDel.RtChild;
                        }

                        //after reassigning the parent to the new replacement node,
                        //we need to null out the parent of the node to delete, 
                        //as well as the right child reference
                        toDel.Parent = null;
                        toDel.RtChild = null;
                    }
                    else
                    { 
                        //the node to be deleted has both left and right children

                        //find the smallest value on the right side of this sub tree
                        //with current being the root
                        BSTNode<T> replacement = FindMin(current.RtChild);

                        //replacements left child now becomes toDel left child
                        replacement.LftChild = toDel.LftChild;

                        //replacement parent now becomes toDel parent
                        replacement.Parent = toDel.Parent;

                        //process the right side of the toDel tree with a queue
                        Queue<BSTNode<T>> adjustment = new Queue<BSTNode<T>>();

                        //start the queue with the right child of toDel
                        adjustment.Enqueue(toDel.RtChild);

                        //while queue is not empty
                        while (adjustment.Count !=0)
                        {
                            //pop first node
                            BSTNode<T> adj = adjustment.Dequeue();

                            if (!adj.Equals(replacement))
                            {
                                //add children of the first node to the back of queue
                                if (adj.LftChild != null)
                                {
                                    //if the node was the original parent of the replacement
                                    //need to null out the reference to the replacement as a child
                                    if (adj.LftChild.Equals(replacement))
                                    {
                                        adj.LftChild = null;
                                    }
                                    else
                                    {
                                        adjustment.Enqueue(current.LftChild);
                                    }
                                }

                                if (adj.RtChild != null)
                                {
                                    adjustment.Enqueue(current.RtChild);
                                }

                                //if the nodes parent is toDel, change the parent to replacement
                                //and add node to replacements right child
                                if (adj.Parent.Equals(toDel))
                                {
                                    adj.Parent = replacement;
                                    replacement.RtChild = adj;
                                }

                                //if the node was a right child of replacement
                                //null out the node parent as replacement
                                //and readd to the tree for proper placement
                                if (adj.Parent.Equals(replacement))
                                {
                                    adj.Parent = null;
                                }
                            }
                        }

                        //change out the reference to the node to be deleted on the parent
                        if (toDel.Parent.LftChild.Equals(toDel))
                        {
                            toDel.Parent.LftChild = replacement;
                        }
                        else if (toDel.Parent.RtChild.Equals(toDel))
                        {
                            toDel.Parent.RtChild = replacement;
                        }

                        //after adjusting the right side of the toDel tree
                        //we need to null out the parent of the node to delete, 
                        //as well as the  child references
                        toDel.Parent = null;
                        toDel.RtChild = null;
                        toDel.LftChild = null;

                    }

                    //node deleted
                    return true;

                    
                }
                else if (compResult < 0)
                {
                    //item is less than root so it needs to be on the left
                    //itemParent = current;
                    //change current to the left child of current                        
                    current = current.LftChild;
                }
                else
                {
                    //item is greater than root so it needs to be on the right
                    //itemParent = current;
                    //change current to the right child of current
                    current = current.RtChild;
                }
            }

            //didn't find node
            return false;
        }

        /// <summary>
        /// Find the minimum value that is in the binary tree
        /// </summary>
        /// <returns>BSTNode element data</returns>
        public T Minimum()
        {
            return FindMin(Root).Data;
        }

        /// <summary>
        /// Find the maximum value that is in the Binary Tree
        /// </summary>
        /// <returns>BSTNode element data</returns>
        public T Maximum()
        {
            return FindMax(Root).Data;
        }

        /// <summary>
        /// Finds the minimum element value that is in the tree starting with a specific node.
        /// </summary>
        /// <param name="startNode">BSTNode that you would like to start with</param>
        /// <returns></returns>
        private BSTNode<T> FindMin(BSTNode<T> startNode)
        {
            BSTNode<T> min = startNode.LftChild;


            if (min == null)
            {
                //if there is no left child of the start node, 
                //then the start node is the minimum value
                return startNode;
            }
            else
            {
                //since we will be checking if min is null to begin with, then we if not null we need to start current at the min
                BSTNode<T> current = min;

                //move down the left side of the tree until we come to a leaf
                while (current != null)
                {
                    if (current.LftChild == null)
                    {
                        min = current;
                        current = null;
                    }
                    else
                    {
                        current = current.LftChild;
                    }


                }
            }

            return min;
        }

        /// <summary>
        /// Finds the Maximum element value that is in the tree starting with a specific node.
        /// </summary>
        /// <param name="startNode">BSTNode that you would like to start with</param>
        /// <returns></returns>
        private BSTNode<T> FindMax(BSTNode<T> startNode)
        {
            BSTNode<T> max = startNode.RtChild;
             

            if (max == null)
            {
                //if there is no right child of the start node, 
                //then the start node is the maximum value
                return startNode;
            }
            else
            {
                //since we will be checking if max is null to begin with, then we if not null we need to start current at the max
                BSTNode<T> current = max;

                //move down the right side of the tree until we come to a leaf
                while (current != null)
                {
                    if (current.RtChild == null)
                    {
                        max = current;
                        current = null;
                    }
                    else
                    {
                        current = current.RtChild;
                    }
                }
            }

            return max;
        }

        /// <summary>
        /// Clears tree
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        public void Clear()
        {
            //by making the root a null node, 
            //we loose all reference to children of the tree
            Root = null;
        }

        /// <summary>
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(BSTNode<T> item)
        {
            //since we will be checking if max is null to begin with, then we if not null we need to start current at the max
            BSTNode<T> current = Root;

            //move down the right side of the tree until we come to a leaf
            while (current != null)
            {
                //temp result of compare so that we don't have to continue comparing within each if statement
                int compResult = item.CompareTo(current.Data);

                if (compResult == 0)
                {
                    //found the node
                    return true;
                }
                else if (compResult < 0)
                {
                    //if result is less than 0 then item has a possbility of being on the left side of tree
                    current = current.LftChild;
                }
                else
                {
                    //if result is greater than 0 then item has a possbility of being on the right side of tree
                    current = current.RtChild;
                }
            }

            //did not find
            return false;
        }


        /// <summary>
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from BST. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(BSTNode<T>[] array, int arrayIndex)
        {
            /*
             *  create empty queue
             *  push root onto queue
             *  while queue is not empty 
             *  -pop first element and add to list
             *  -if element has left and right node, add them to queue
             *  
             */

            Queue<BSTNode<T>> que = new Queue<BSTNode<T>>();
            List<BSTNode<T>> inOrder = new List<BSTNode<T>>();
            BSTNode<T> current;
            que.Enqueue(Root);

            while (que.Count != 0)
            {
                current = que.Dequeue();

                if (current.LftChild != null)
                {
                    que.Enqueue(current.LftChild);
                }

                if (current.RtChild != null)
                {
                    que.Enqueue(current.RtChild);
                }

                inOrder.Add(current);
            }

            //move the sorted list to the array
            inOrder.CopyTo(array);
            

            
            
        }


        /// <summary>
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BSTNode<T>> GetEnumerator()
        {
            return this.GetEnumerator();
        }


        /// <summary>
        /// Note: Must have this method due to implementing ICollection
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {

            //TODO:  Should we be able to do foreach loops on this?
            BSTNode<T> current = Root;
            //BSTNode<T> previous = null;

            while (current != null)
            {
                yield return current.Data;
                current = current.LftChild;
            };

            //while current is not null go through the tree
            //while (current != null)
            //{
            //    //temp result of compare so that we don't have to continue comparing within each if statement
            //    int compResult = this. item.CompareTo(current.Data);

            //    //if root is not empty compare to see if the new item is less than or greater than
            //    if (compResult == 0)
            //    {
            //        //if equal do nothing because we don't want duplicates
            //        return;
            //    }
            //    else if (compResult < 0)
            //    {
            //        //item is less than root so it needs to be on the left

            //        //assuming that the current node doesn't have any children
            //        itemParent = current;
            //        //change current to the left child of current                        
            //        current = current.LftChild;
            //    }
            //    else
            //    {
            //        //item is greater than root so it needs to be on the right
            //        itemParent = current;

            //        //change current to the right child of current
            //        current = current.RtChild;
            //    }
            //}
        }

        
    }
}
