# Tool development assignment
This project is based around a tool development assignment, Where the task was to develop a tool.
## Primary Tool
The primary tool i made was to have a ship editor, A editor window allowing for quick access for all of the ship scriptable objects in the project. In this window you can select between all the ships, create new ones and delete the currently selected one.

The Tool also has the avaliability to edit the settings for the ship object, add different components that are also separated into separate scriptable objects, add and delete components and edit each individual components own stats.
As the components are also scriptable object their settings persist throughout every instance of them when you edit on a single ship.

## Vg points
* Utilize advanced features of Unity editor tools, such as custom inspectors or editors, to enhance the user experience and make the tool more powerful and efficient. 
The tool was made with the uiToolkit in order to have a fully custom editor window for easy access, and ui toolkit also alows for quick changes for things like editing the bounds the variables are alowed to be in.

* Incorporate error handling, undo/redo functionality, and other best practices to improve the robustness and reliability of the tool.
I'm not sure exactly what are best practices and such, but undo/redo seems to work since it's implementing a lot of base unity stuff. and everything seems fairly robust and reliable as far as i can tell.

* Create additional functionality or expand on the existing functionality to make the tool more useful and versatile.
Again not entierly sure what this would entail, But it's mostly built out in order to make sure that all of the functionality i had implemented so far is in there.

* Reflect on the process of creating the tool and provide insightful and thoughtful feedback on how the use of ScriptableObjects and Unity editor tools impacted the development process.
I had a nice realization about scriptable objects usability when i realized that their primary purpouse is to remain a solid connection even if you send the instance to different references. so that for example the uiManager can get sent the base instance at the same time as the player during setup and any changes the player then makes is visible to the uiManager.

The process of creating the tool itself was quite difficult as i was fairly lost in everything about the ui toolkit and was a bit lost in what you concidered to be a Tool for game development. Lacked a bit of introduction to the subject. There was also issues regarding the documentation with everything about the ui toolkit as it's a bit all over the place since it's been through so many itterations recently.

* Create a well-organized and easy-to-user interface, making sure that all functionalities are intuitive and easy to find
That's a matter of opinion in the most cases, but i personally believe the tool itself is quite easy to use and designer friendly. And the functionalities are all displayed and should be fairly understandable.

* Add functionality to the tool which makes it more reusable, like exposing methods or properties to the user that are not hard-coded.
I lacked the time and effort to make every part entierly reusable so a fair few things in the editor window is fairly hardcoded to use the same objects and types. but it should'nt be too hard to refractor into a all around component tool if i where to have more time and effort to want to put into this. Check back later in case you're interested and i might have updated things.

* Show evidence of user testing and iteration based on feedback.
No real time or people i bothered involving in this, sorry :)

* Make the tool a Unity package so it can be easily shared with others, and document the package
As it's still fairly hardcoded i would not get around to this yet, i will be looking into it later though.

## Remaining issues
* the different ships are as of now not implemented to be able to get chosen by the user before game start.
* There's no asteroids to shoot
* There was no scriptable object made for the bullets, so you can adjust damage.
* There's no ui displaying the current battery level of the ship
* The batterys don't create a new instance per ship spawned, so they'd be using the same current battery value at this time.