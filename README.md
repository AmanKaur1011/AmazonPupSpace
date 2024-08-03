# AmazonPupSpace README

Welcome to **AmazonPupSpace**! 🎉🐾

AmazonPupSpace is a fun and engaging application designed exclusively for Amazon employees to share their beloved dogs and the amazing tricks they learn. Whether your dog can roll over, fetch, or perform a high-five, this is the perfect place to showcase their talents and connect with fellow co-workers who just happen to be dog lovers.

## Table of Contents
1. [Features](#features)
2. [Getting Started](#getting-started)
3. [How to Use](#how-to-use)
4. [Technical Details](#technical-details)
5. [Team Contributions](#team)

## Features

- **Profile Management**: Each employee has a profile where they can add their dog’s details and create posts when their dog learns a trick.
- **Post Tricks**: Share images and descriptions of your dog’s tricks.
- **Departmental Groups**: Employees are assigned by departments.
- **Commenting**: Comment on other employees’ posts to show your appreciation and interact.
- **Image Upload**: Easily upload and display images of your dog performing tricks.
- **Admin**: CRUD for employees, departments, dogs, tricks

## Getting Started

1. **Access the Application**: clone the repo, add the "App_Data" folder and run the comand "update-database" in the Nuget Package Manager Console

## How to Use

### Posting a Trick

1. **Go to the Post Section**: Click on the **Create Post** button.
2. **Upload an Image**: Choose an image file of your dog performing the trick.
3. **Add a Description**: Write a brief description of the trick and any relevant details.
5. **Submit**: Click **Post** to share with the community.

### Commenting on Posts

1. **View Posts**: Browse through posts 
2. **Add a Comment**: Click on the **Comment** button below a post and enter your message.
3. **Submit Comment**: Press **Send** to add your comment to the post.

## Technical Details

- **Frontend**: Built with C# and styled using bootstrap.
- **Backend**: Powered by an ASP.Net backend.
- **Authentication**: ASP.Net built-in user functionality.

## Team

- **Aman**: Employee + Department CRUD, Employee connection to Dog Table including updating views and controllers
- **Sumin**: Post + Comment CRUD, 
- **Alejandro**: Dog + Trick CRUD

Thank you for using AmazonPupSpace! We hope you enjoy sharing and discovering all the fantastic tricks that your canine companions have to offer. 🐶✨

Happy posting!

*The AmazonPupSpace Team*
