# Together
### A solo game for Games and Social Justice about relationships and how we process them. 

My first solo project in Unity -- The code for this is pretty sub-par by my current standards. 

I did go back and make some improvements, but the core of the code remains the same. The worst offense is that far too much of it makes heavy use of update loops and bools to trigger events, rather than just having the functions called when the bool would be updated. However, considering that this was my first time really using Unity, didn't have a full grasp on coroutines, and thought I could only serialize `GameObjects` for some reason and thus was calling `GetComponent` everywhere, the code isn't terrible. 
