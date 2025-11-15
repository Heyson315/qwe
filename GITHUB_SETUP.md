# GitHub Setup Guide - HHR CPA Project

This guide will help you set up your project on GitHub and enable seamless collaboration across different environments (Visual Studio, VS Code, web, etc.).

## Current Status
- ✅ Repository already exists: `https://github.com/Heyson315/qwe`
- ✅ Local repository on branch: `master`
- ✅ Remote configured: `origin`

## Quick Setup Steps

### 1. Commit All Current Changes

```bash
# Check current status
git status

# Add all new files
git add .

# Commit with descriptive message
git commit -m "chore: Add project management infrastructure and documentation"

# Push to GitHub
git push origin master
```

### 2. Create Development Branch

```bash
# Create and switch to develop branch
git checkout -b develop

# Push develop branch to GitHub
git push -u origin develop

# Switch back to master
git checkout master
```

### 3. Set Up Branch Protection (On GitHub)

1. Go to: `https://github.com/Heyson315/qwe/settings/branches`
2. Click "Add rule" for `master` branch:
   - ☑ Require pull request reviews before merging
   - ☑ Require status checks to pass before merging
   - ☑ Include administrators
3. Repeat for `develop` branch

### 4. Clone in VS Code (Optional)

If you want to work in VS Code:

```bash
# Navigate to your projects folder
cd C:\Users\HassanRahman\source\repos\

# Clone (if starting fresh in new location)
git clone https://github.com/Heyson315/qwe

# Open in VS Code
cd qwe
code .
```

## Working Across Different Environments

### Visual Studio
- Open: `qwe.sln`
- Git integration: Team Explorer
- Build: Ctrl+Shift+B
- Run: F5

### VS Code
- Open: Folder `C:\Users\HassanRahman\source\repos\qwe`
- Extensions recommended:
  - C# Dev Kit
  - GitLens
  - Azure Account (for deployment)
- Terminal: Built-in for git commands

### GitHub Web (Codespaces)
```bash
# From your repository page
# Click: Code > Codespaces > Create codespace on master
# This gives you VS Code in the browser!
```

## Daily Workflow

### Starting New Work

```bash
# 1. Switch to develop
git checkout develop

# 2. Get latest changes
git pull origin develop

# 3. Create feature branch
git checkout -b feature/my-new-feature

# 4. Make your changes...

# 5. Commit changes
git add .
git commit -m "feat: Add my new feature"

# 6. Push to GitHub
git push origin feature/my-new-feature

# 7. Create Pull Request on GitHub
# Go to: https://github.com/Heyson315/qwe/pulls
# Click "New Pull Request"
# Select: base: develop <- compare: feature/my-new-feature
```

### Syncing Changes

```bash
# Pull latest changes from GitHub
git pull origin master

# Or if on develop branch
git pull origin develop

# Update all branches
git fetch --all
```

## Syncing Between Visual Studio and VS Code

Both use the same local Git repository, so:

1. **Commit in Visual Studio**
   ```bash
   # Automatically syncs to local repo
   # Push to share with others
   ```

2. **Pull in VS Code**
   ```bash
   git pull
   # Your changes appear!
   ```

3. **Works both ways** - Changes committed in either tool are available in the other

## Project Structure on GitHub

```
https://github.com/Heyson315/qwe/
├── .github/
│   ├── ISSUE_TEMPLATE/
│   │   ├── bug_report.md
│   │   ├── feature_request.md
│   │   └── security_issue.md
│   └── pull_request_template.md
│
├── qwe/                          # Main project
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Views/
│   └── ...
│
├── qwe.Tests/                    # Test project
│
├── README.md                     # Project overview
├── API_DOCUMENTATION.md          # API reference
├── DEVELOPMENT_GUIDELINES.md     # Development standards
└── GITHUB_SETUP.md              # This file
```

## GitHub Features to Enable

### 1. Issues
Track bugs and features:
- Go to: Settings > General > Features
- ☑ Issues

### 2. Projects
Kanban board for task management:
- Go to: Projects > New Project
- Choose: Board template
- Columns: To Do, In Progress, Done

### 3. Actions (CI/CD)
Automated testing and deployment:
```yaml
# .github/workflows/build.yml (to be created)
name: Build and Test
on: [push, pull_request]
jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v2
      - name: Build
        run: dotnet build
      - name: Test
        run: dotnet test
```

### 4. Wiki
Additional documentation:
- Go to: Wiki > Create first page
- Add setup guides, FAQs, etc.

## Collaboration Features

### Working with Team Members

```bash
# Add collaborators
# Settings > Collaborators > Add people

# They can then:
git clone https://github.com/Heyson315/qwe
git checkout -b feature/their-feature
# Make changes
git push origin feature/their-feature
# Create PR
```

### Code Review Process

1. **Developer creates PR**
2. **Reviewers comment** on code
3. **Developer addresses feedback**
4. **Reviewer approves**
5. **PR merged to develop**
6. **Feature branch deleted**

## GitHub Desktop (Alternative)

If you prefer GUI over command line:

1. Download: https://desktop.github.com/
2. File > Add Local Repository
3. Select: `C:\Users\HassanRahman\source\repos\qwe`
4. Use GUI for commits, pushes, branch management

## Troubleshooting

### Push Rejected
```bash
# Someone else pushed changes
git pull --rebase origin master
git push origin master
```

### Merge Conflicts
```bash
# Edit conflicted files
# Look for <<<<<<< markers
# Choose which changes to keep
git add .
git commit -m "fix: Resolve merge conflicts"
git push
```

### Forgot to Create Branch
```bash
# Move changes to new branch
git stash
git checkout -b feature/my-feature
git stash pop
git commit -m "feat: My feature"
```

### Need to Update from Master
```bash
git checkout feature/my-feature
git rebase master
# Resolve any conflicts
git push --force-with-lease
```

## Quick Reference Commands

```bash
# Status
git status
git log --oneline -10

# Branching
git branch                    # List branches
git checkout -b new-branch    # Create and switch
git branch -d old-branch      # Delete local branch

# Syncing
git fetch --all              # Get all remote changes
git pull origin master       # Pull and merge
git push origin branch-name  # Push branch

# Undoing
git reset HEAD~1             # Undo last commit (keep changes)
git checkout -- file.txt     # Discard file changes
git clean -fd                # Remove untracked files

# Viewing
git diff                     # See unstaged changes
git diff --staged            # See staged changes
git show commit-hash         # View specific commit
```

## Integration with Visual Studio

### Team Explorer (Visual Studio)
1. View > Team Explorer
2. **Home** - Overview
3. **Changes** - Stage and commit
4. **Branches** - Manage branches
5. **Sync** - Push/Pull
6. **Pull Requests** - View PRs

### Git Features
- **Built-in merge tool** - Resolve conflicts visually
- **Blame/Annotate** - See who changed what
- **History** - Visual commit history

## Next Steps

1. ✅ Commit and push current changes
2. ⬜ Set up branch protection rules
3. ⬜ Enable GitHub Issues
4. ⬜ Create first issue using templates
5. ⬜ Set up GitHub Project board
6. ⬜ Invite collaborators (if any)
7. ⬜ Set up GitHub Actions (CI/CD)

## Resources

- **GitHub Docs**: https://docs.github.com/
- **Git Cheat Sheet**: https://education.github.com/git-cheat-sheet-education.pdf
- **Visual Studio Git**: https://learn.microsoft.com/visualstudio/version-control/
- **VS Code Git**: https://code.visualstudio.com/docs/sourcecontrol/overview

## Support

If you encounter issues:
1. Check this guide
2. Search GitHub Issues
3. Consult Git documentation
4. Ask in team chat

---

**Ready to push to GitHub?** Run these commands:

```bash
git add .
git commit -m "chore: Add comprehensive project management setup"
git push origin master
```

Then visit: https://github.com/Heyson315/qwe to see your project online! 🚀
