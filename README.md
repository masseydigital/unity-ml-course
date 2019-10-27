# unity-ml-course

Course repository for A Beginner's Guide to Machine Learning with Unity by Penny de Byl on Udemy.

## Genetic Algorithms

### DNA Inspired Structures

Genetics: Concentrates on the transmission of traits from parents to offspring.  This includes behavior traits such as temperament and intelligence.  These traits are stored in _chromosomes_.  Chromosomes contain a list of traits that are stored in a gene.  Evolutionary computing attempts to mimic this phenomenon through the use of computing.

Each trait is mapped to a binary array that represents a chromosome.  Each population is put through a test or series of tests and rated on some statistic such as speed, resources collected etc.  Once the test is run, some percentage of organisms are combined to create the next generation of testers.

One example of a genetic algorithm seen in nature is _camouflage_.  In nature, animals that are able to blend into their environment are able to avoid capture by predators more easily.  Over time, this causes the population of animal to fit their environment better.

In nature, _mutations_ sometimes occur which can either serve as a benefit or inhibitor to an organisms survivability.  This can be achieved in code by introducing a random chance to set a gene to a completely random value.

A _fitness test_ determines which set of the population survives and which set dies.

### Repo Scenes

**_Camouflage:_** This scene generates colored persons on the screen which the user can click.  The algorithm uses the sequence of color selection to determine the next generations color.  The idea is to mimic how camouflage works in nature where creatures that can mimic their surroundings survive longer.

**_Maze:_** This scene trains the agents to move through the maze as far as possible.  The agents have the ability to move in different directions with their gene and will prioritize movements that maximize how far they can go.

**_Movement:_** This scene trains agents to stay on the extended platform by performing 1 of 6 movements types.  The result is that agents that move straight, crouch, or jump move through each generation.

**_Platformer:_** This scene trains agents to perform actions that keep them on the platform.  The fitness function prioritizes actions that keep them on the platform and additionally rewards agents that travel further distance.

**_Flappy:_** This scene trains agents to move through a Flappy Bird game by performing up down controls.  THe genes for the birds are length 5.  1: up, 2: down wall encountered, 3: top, 4: bottom, 5: default force when nothing is sensed.  The fitness function includes distance traveled which rewards birds that move through the maze and continue going.

### Additonal Articles

https://www.gamasutra.com/blogs/MichaelMartin/20110830/90109/Using_a_Genetic_Algorithm_to_Create_Adaptive_Enemy_AI.php

http://rednuht.org/genetic_walkers/

https://www.wired.com/2010/11/genetic-algorithms-starcraft/

## Perceptrons

The _perceptron_ is a fundamental algorithm behind the functioning of a neural network and attempts to model the functioning of a neuron.

The act of learning in a neuron happens through _reinforcement_.  Positive behaviors strengthen the likeliness of firing and negative behaviors reduces the likeliness.

A perceptron takes in inputs, multiplied by weights and an activation function to produce an output.  Learning occurs in the weight values.  An extra weight value called the _bias_ helps move the algorithm to its goal output.  

The _activation function_ determines the output of the perceptron based on the weights and the inputs.  

A _training set_ is produced to check the networks correctness.  Each output of the network is checked to the training set and produces an _Error_.  Each iteration through the training set is called an _Epoch_.

A perceptron is considered to be a _binary classifier_ in that it can separate values into two areas of data.

A single perceptron can not handle the XOR statement.  This is because there is not a good way to draw a line which can separate the two values.  IN future sections we will create more advanced networks which are able to mimic this functionality.

### Repo Scenes

**_Perceptron:_** This scene shows the basic set-up for creating a Perceptron in Unity.  The perceptron is representing an OR gate, and after some iterations of training should mimic the output.

**_Classification:_** This scene shows how to set-up a classifier which buckets points into one type of object or another.  The scene will create a graph which lists all of the points in the training set and then draws a best fit line based on the output of the training.

**_Learning:_** This scene shows how to change the Perceptron class to perform training while the scene is running.  In the scene, balls can be thrown using the 1,2,3,4 keys.  The scene is set-up so that the player learns to dodge red balls.  After a few iterations of throwing objects, the player should always duck only when a red ball is thrown.  You can also  use the "s" and "l" keys to save and load the current weights that have been trained.

## Artificial Neural Networks

Artificial Neural Networks (ANN) take the perceptrons (neurons) that we programmed in the previous section and links them together to create a network.  This is also referred to as _deep learning._

In a ANN, networks are separated into 3 layers.  (1) Input Layer, (2) 1 or more Hidden Layers, (3) Output Layer.

_Backpropogation_ is the process of taking the error in the output and pushing it back through the network to adjust the weights of the network.  

ANNs are best suited for classification problems, data processing, robotics, computer controls, and statistical analysis.  In games, they are typically not used due to difficulty programming as well as un-predictable behavior.  An example of ANNs in action for games is Forza Horizon using it to simulate more realistic driving behavior.  

An _Error Gradient_ is assigning an amount of error to a specific weight in a neuron.

_Normalization_ is the process of processing the data to make it more meaningful and manageable by the machine learning algorithms.  Normalization also brings the values of the inputs into ranges between 0 and 1.  This is useful because many of the activation functions given work best between 0 and 1.

_Local Optima_ are low points in the error gradient that either trap the network into a shallow hole or skip over the lowest point for error.

### FAQs

**1. What activation function should I use?**
The gradient of the error being backpropogated is the slope of the line used to calculate the error.  The step function cannot be used to calculate an XOR gate. The gradient of a line is its slope.  Flat lines have a gradient of 0 and straight up and down slopes have a gradient of infinity.  This is why we used the sigmoid function.  It has a nice gradient between -2 and 2.  Any neuron beyond that number will have trouble training.  This is called the _vanishing gradient_.

Tanh is another nice function to use, and looks similar to the Sigmoid function, but with a steeper slope.

ReLU (Rectified Linear Unit) is another good function to use for positive weights.  This is called a sparsely activated network.  It has no gradient below 0.

Leaky ReLU has a slight slope in the negative direction.  This allows for some training below 0.  

A common set-up for networks is to use ReLU or Leaky ReLU in the hidden layers and Sigmoid in the output layer.

https://en.wikipedia.org/wiki/Activation_function

**2. How many layers do I need?**
Each layer produces its feature boundary.  What this means is that each layer is focused on a specific feature that the network is learning.  If you don't know what features your network should be learning, it might be helpful to start with a layer and start to increase the number of hidden layers.

**3. How many neurons do I need?**
For the input layer you need as many neurons as there are inputs.  For the output layer you need as many output layer as there are inputs.  For the hidden layers a common technique to pick a value between the number of inputs and the number of outputs.  Another rule could be less than twice the number of neurons in the input layer.  If you do not have enough neurons in the hidden layer, your network will experience _underfitting_.  if you have too many neurons in the hidden layers, you will experience _overfitting_.  The problem with overfitting is that when new values are introduced in your system, they may not fit into the training buckets.


### Extra Readings

http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.161.3556&rep=rep1&type=pdf

https://jeremykun.com/2011/08/11/the-perceptron-and-all-the-things-it-cant-perceive/

https://medium.com/the-theory-of-everything/understanding-activation-functions-in-neural-networks-9491262884e0

### Repo Scenes

**_Intro:_** This scene is the introduction to programming ANNs from scratch.  The example scene builds an XOR gate utilizing an ANN. 

**_Pong_:** This scene trains a paddle utilizing an ANN.  Things that the player may consider when making a move is the Balls position, the Balls Direction & Speed, the Paddles position, and the Paddles velocity.  This translates to (6) inputs that can be adjusted - (1) Ball X, (2) Ball Y, (3) Ball Velocity X, (4) Ball Velocity Y, (5) Paddle X, and (6) Paddle Y.

**_PongTwo:_** This scene pits the player against the AI.

**_Racer:_** This scene trains a neural network to allow for an AI car to drive around a track.  It does this by calculating the distance to the walls in front, directly to the sides, and at 45 degree angles to determine if it should move.  Once the network is training you can save the weights out and use them if your training was successful.  An optimal visibleDistance appears to be 50.0 - it seems like when I did 200 that the car didn't work.