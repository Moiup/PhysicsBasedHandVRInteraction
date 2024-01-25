# **Pure Physics-Based Hand Interaction in VR**

<!-- ![](imgs/lift2_new.png) ![](imgs/throw1_1.1.1.png) ![](imgs/throw2_1.1.2.png) -->

<!-- <style>
    #top-img{
        display: flex;
        flex-direction: row;
        justify-content: space-evenly;
    }
</style> -->

<div id="top-img" display="flex" flex-direction="row" justify-content="space-evenly">
<img src="imgs/lift2_new.png" width="250" height="auto">
<img src="imgs/throw1_1.1.1.png" width="250" height="auto">
<img src="imgs/throw2_1.1.2.png" width="250" height="auto">
</div>

## **Abstract**

Interaction in Virtual Reality is still mainly done using controllers. However, since the early 2000s, there has been a desire to find a way to interact within the virtual environment using only our hands. Pinch motion detection was introduced to detect a grasping action. Since then, hands' motion capture has been highly developed until being directly integrated in VR headsets. Thus, multiple research projects were done in order to exploit this technology for better grasping techniques. Recent works tend to bring physical hand interaction to VR. However, they introduce physical heuristics to determine if an object is grasped or not, but in reality, motion is purely kinematic. In our paper, we introduce a purely physical method based on Hooke's spring law that eliminates the need for a grasping mode. Additionally, we incorporate visual feedback methods to compensate for the absence of the sense of touch. Consequently, with this approach, we can lift objects, throw them, stack them, and interact with them naturally. We carried out extensive tests with several people who had no previous experience, to validate our technique.