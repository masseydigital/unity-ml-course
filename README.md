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

### Repo Scenes

**_Perceptron:_** This scene shows the basic set-up for creating a Perceptron in Unity.  The perceptron is representing an OR gate, and after some iterations of training should mimic the output.

**_Classification:_** This scene shows how to set-up a classifier which buckets points into one type of object or another.  The scene will create a graph which lists all of the points in the training set and then draws a best fit line based on the output of the training.

**_Learning:_** This scene shows how to change the Perceptron class to perform training while the scene is running.  In the scene, balls can be thrown using the 1,2,3,4 keys.  The scene is set-up so that the player learns to dodge red balls.  After a few iterations of throwing objects, the player should always duck only when a red ball is thrown.  You can also  use the "s" and "l" keys to save and load the current weights that have been trained.
