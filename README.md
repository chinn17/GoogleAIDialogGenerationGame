# GoogleAIDialogGenerationGame

# First time use
1. Clone the repo.
```
git clone
```

2. Open the project in Unity
```
Launch Unity 
Select “Open“ to open an existing project
Select pizzaMaker folder
```


# Workflow Tips
## Branching

1. First switch to development branch, and do a git fetch and pull to update your local repo to the latest.
```
git checkout development
git fetch --all
git pull
```
2. Create a new branch for the specific feature you are working on.
```
git checkout -b my-new-feature-branch
```

3. Track all the new files by adding them to the repo and commit your changes.
```
git add .
git commit -a -m "a commit message"
```

4. Push your branch to the repo and create a pull request from github
```
git push origin my-new-feature-branch
```

5. Make sure to switch back to the dev branch and create another branch before starting to work on another feature.
```
git checkout dev
```
