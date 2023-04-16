# The *Unforgettable* Memory Game
A memory game so artistic that it's simply unforgettable. 
I have tried using the same-ish art style on all cards so that the cards are similar, thus making it a bit harder to remember which is which. 
Points are based on the time it took to find a pair. Each pair brings 10 points, multiplied by the time factor, where the time factor TF goes from 1 under 45 seconds, to 0.5 over 120 seconds [with additional steps between].


## The Spec

Basic Mechanics

 - [x] Open two cards per turn
 - [x] Opening same two cards - Cards stay open - Add Points to player
 - [x] Opening different two cards - Cards close after delay
 - [x] Opening any two cards - Increase number of turns
 - [x] Implement elapsed time since game started
 - [x] Button for restarting game
 - [x] Shuffle cards and their positions on game start/restart
 - [ ] Background music
 - [ ] Sound effect after opening any one card
 - [ ] Build game for mobile and for PC
 - [x] Put project on git and have normal commit messages
 - [x] End of game is when player opens all cards
 - [x] At end of game, congratulate player based on number of points
 - [x] Number of points is multiplied by elapsed time factor [eg, seconds under 60 is 1 \* points, seconds between 60 and 120 is 0.9 \* points..]
