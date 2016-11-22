![Logo](http://raxterworks.com/Prototypes/VJ_static_small.png)

# Cubes-upon-Cubes-VJ-Software
This is the VJ software I'm using at the moment. Constantly updating as I do shows. 

Please use for your general VJ-ing education. I found it was very hard to find VJ software online, so I present this. Flaws and all! I take no responcibility for the shoddy code. 
Whilst I can code at some level of decent standard, this is a VJ hacky jam thing I made and have had no time to refactor. 
It's my special little untidy monster ****scruffles head**** so you'll have to accept it as that ^ _ ^

Also check this out, helped me a lot when I was starting out https://medium.com/@superdajk

I hope that my work might give you some technical insight or some inspiration :)

Feel free to contact me if you have questions about the code.
I can't promise I'll be able to answer but I'll certainly try if I have time!

Oh right, if you want me to VJ around Europe somewhere let me know ;)

Stay cool, VJ troopers <3

Twitter: @raxterbaxter 
or 
Email: richard [dot] jonathan [dot] baxter [at] gmail [dot] com

# Technical Info / Getting started
If run just as an app, it needs an xbox (or similar) controller to run. I'll list all the controls here someday (but today is not that day). I think all the buttons and sticks do something. Numbers 1-9 on the keyboard change the colours.

Soooo, yeah the code is a mess. Start off in "Render Texture Fun" scene and start looking around from there. 

The main classes you want to look at are 

CubeTunnlesController.cs and CubeTunnle.cs - Most of the movement stuff is here

BigBadController.cs and its controller, InputZentral.cs - Has all the variables that the Cube Tunnle Controller is given (and a whole lot of variables that aren't anymore)

BeatFinder.cs - I adapted this from a tapper file in https://github.com/memo/ofxMSABPMTapper and ported it to Unity... err badly.

ColorScheme.cs - These are used as prefabs to store the colour and lighting info

The branches are usually pretty dead and are offshoots that I don't feel like deleteing. If this is bad practise then... *shrug* guess it's bad then?

# Licence Stuff
So basically:

Please do not use for performances or for monetary gain/commercial use, unless you have my permission.
Please email me if you have any questions regarding any potential commercial use.

I'll probably be ok with you taking bits and pieces of the code (tho, seriously I would not recommend taking too much... it's not good code) so long as its isolated bits of code used in your own original work and that the artistic direction of your work is your own, i.e. not a copy of or heavily resembling what I've done here. If unsure just message me to check :)

I take no responsibility for this code or anything this code may or may not do.

If at all possilble, VJ more.
