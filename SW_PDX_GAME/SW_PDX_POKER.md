This Repository is part of an effort to develop an online cash poker game played among friends. 

This project uses Unity 2019.3.6f1 and C# programming laguage. A multiplayer Networking platform "Socket-Weaver" (https://socketweaver.com/) 

The base of this project was taken from a socket-weaver tutorial for playing Go-Fish with multiplayers over a network connection. 
I have the go-fish repo here: https://github.com/MegaCoulomb/GoFish
There is a readme.md file there as well with links to the youtube videos.

because I used the go-fish code as a jumping off point to get started there may still be some functions that need to be ripped out or 
renamed. 

The youtube videos are good for explaining how the lobby works and how the multiplayer setup works. 

so the basic path is to get the local player vs AI up and running correctly, then make the minor changes needed to multiplayer.cs. 

I am not sure Socket-Weaver is the server solution I want to use but we will see what i can find later; for now it works.

Currently I have up through pre-flop betting coded accurately. There is a very basic betting function with a slide bar. the math 
does not work totally correct right now but that is a minor detail I planned to circle back to.

so things still needed (this is a fluid list of features):
  1) dealing (all three deals can be done from one function i beleieve) - dealer button,..burn card, deal proper amount of cards 
      --> pseudo code is mostly written for this. still need to write pseudo code for dealer button, and modify state machine
      so we can have a previous state input to make it a Mealy type state machine.  --> I am currently working on this...
  2) betting needs to be finished & expanded - currently slider and bet button works but check/fold buttons are not hooked up and
     math is not functioning correct yet (visually), and needs an accounting function to ensure pot is good and take into account 
     BB SB positionss...
  3) Function to determine end of hand
  4) function to determine winning hands
  5) start next hand...
     
     This is a super abbreviated list right now. lets add to it as we knock off these items.
     
     Happy Coding!
