# GE2-Space-Assignment
This is the repository for my Game Engines 2 Assignment. 

Name: Ciaran McHale

Student Number: C16486904

Class Group: DT228/4

For this Assignment I recreated the intro to Star Wars A new Hope

Two Spaceships come flying in off screen. The smaller ship is fleeing from the larger Imperial Destroyer Ship. The camera remains fixed as the two space crafts enter into view
and watches them pass by.

For this project I had looked at a previously done star wars project and got the idea for this project from that. For the code which I didnt utilise form class I foudn through the unity forum as well as the website vuforia. 

## References

1. vuforia developer library https://library.vuforia.com/
2. Unity forum: https://forum.unity.com/
3. Unity Manual Docs - Internet [https://docs.unity3d.com/Manual/UnityManual.html]

## **Description of the project:**

For this project I structured classes into seperate directories to make the the use of the code easier. The main points of the project were to set a spawn point off camera for both ships. From there they would head a specific direction with the larger ship firing at the smaller ship. Initally when coming up with the idea for this project I was planning on having a large battle to take place afterwards with multiple smaller ships however I could not complete the full scope. The project started wiht me rendering the spaceships and have them spawn into the scene. From there I created turret spawner classes as well and focused on creating classes to handle ships targeting one another. Then  once that was complete I had controllers to handle spawning, camera orietnation and the health of the ships. As I previously said I had planned on creating multiple ships and different camera angles however it became difficult for me to complete my origianl scope for the project. 

## **Development:**

For this project I hadn't developed any steering behaviours for the ships. I utilised vectors, raycasting and trasnfroming along with other material I had learned from class. Firstly I went about creating a controller folder for handling of the various different fucntions such as the spawning of the ships, destruction of ships and projectiles along wiht many other various parts. From there I developed the class for the spacehsips for handling speed and torque. Other clases included developing a targeting system that showed potential targets and tagged the said target. For the scene the two ships follwed a specified direction with the same speed. Other clases which I implemented were for interfaces for explosiosns, projectile speed and other necessary interfaces. I was not able to implement all of the interfaces as hoped. I developed most of the classes my self but I also utilised other code and classes from sources which I referenced above. The art assets used in this project were created through the use materials from unity. 


![alt text](https://www.denofgeek.com/wp-content/uploads/2016/04/star-wars-opening-main_0.jpg)


[![](http://img.youtube.com/vi/lrSFy0DbFaA/0.jpg)](http://www.youtube.com/watch?v=lrSFy0DbFaA "")
