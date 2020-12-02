# SpatialGame
This codebase reflects the most recent version of the game for training spatial skills, Homeworld Bound, that I have developed as part of my dissertation. Unity version 2018.3.5f1 was and is being used for the game's development. 

While the target population has varied over the years (from children to high schoolers and adults), the current target population is high school and college-age students with low spatial skills who could benefit from spatial training that would potentially improve their performance and confidence in spatially demanding early STEM coursework.

Currently, the game can be run online in the browser, with a database optionally attached to record detailed in-game player behavior data. The most recent commits include a new autosave system, which works on either local Windows machines or in WebGL when attached to a server database, depending on what code is commented/uncommented. Currently, the code for the online autosave is active. However, there are a few bugs right now:

B1: Autosave for collection of Key1 parts (last level of playable content) does not work. 
B2: The audio is currently stuttering on Chrome. 

The game is currently playable, though. In the near future, I hope to fix these bugs and will update this page once I do so.

A recent online build of the game with the current version of the autosave feature can be played here: http://games.spatial.cs.illinois.edu/auth?netID=<replaceWithSomeUniqueID\>

This URL format is necessary since the game creates a new autosave file using whatever you put for `someUniqueID` (so you can resume where you left off if you close the browser or refresh). If you would like to try out the game without creating a save file, you can use this url instead: http://games.spatial.cs.illinois.edu. Note, however, that there may be a couple error popups in the browser when you first start playing - check the option to prevent future popup error messages when you see it. The popups will not cause the game to break but will keep popping up if you don't check this option.


*****************************************************************************************************************************
Credit for music and sound effects goes to the following artists:

Music:

"Danse Morialta", "Dreamer", "Gymnopedie No. 1", "Pepper's Theme"
Kevin MacLeod (incompetech.com)
Licensed under Creative Commons: By Attribution 3.0
http://creativecommons.org/licenses/by/3.0/

"CreationM", "CreationBatteryLow" by Sebastian Saraceno (collaborator, composed specifically for this game)

Absolutely Free Music by Vertex Studio (https://assetstore.unity.com/publishers/2053)


Sound Effects:

Fusion2 by wildweasel (https://freesound.org/people/wildweasel)

MemoryMoon_space-blaster-plays by suonho (https://freesound.org/people/suonho)

Game Sound Selection by Bertrof (https://freesound.org/people/Bertrof)

sf3-sfx-menu-select by broumbroum (https://freesound.org/people/broumbroum)

Spaceship by tyops (https://freesound.org/people/tyops)

hit_002 by leviclaassen (https://freesound.org/people/leviclaassen)

Casual Game Sounds - Single Shot SFX Pack by Dustyroom (dustyroom.com)

Fantasy SFX for Particle Distort Texture Effect Library by Moonflower Carnivore (https://assetstore.unity.com/publishers/12261)

SciFi UI Sound FX by Bright Shining Star (https://assetstore.unity.com/publishers/9592)


