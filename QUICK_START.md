# Quick Start - Working with GitHub

## ✅ Setup Complete!

Your project is now fully set up on GitHub:
- 🔗 **Repository**: https://github.com/Heyson315/qwe
- 🌿 **Branches**: `master` (production) and `develop` (development)
- 📝 **All files pushed** successfully!

---

## 🚀 Quick Commands Reference

### View Your Project Online
```bash
# Open in browser
start https://github.com/Heyson315/qwe
```

### Daily Workflow (Most Common)

```bash
# 1. Start your day - Get latest changes
git checkout develop
git pull origin develop

# 2. Create a new feature
git checkout -b feature/my-feature-name

# 3. Make your changes in Visual Studio or VS Code
# ... edit files ...

# 4. Save your work
git add .
git commit -m "feat: Brief description of what you did"

# 5. Push to GitHub
git push origin feature/my-feature-name

# 6. Create Pull Request on GitHub
# Visit: https://github.com/Heyson315/qwe/pulls
# Click "New Pull Request"
```

### Switch Between Computers/Locations

```bash
# On new computer, clone the repository
git clone https://github.com/Heyson315/qwe
cd qwe

# Open in Visual Studio
start qwe.sln

# Or open in VS Code
code .

# Pull latest changes
git pull origin develop
```

---

## 💻 Working in Different Editors

### Visual Studio (Current)
1. Open `qwe.sln`
2. Use Team Explorer for Git operations
3. Changes automatically sync to local Git

### VS Code
1. Open folder: `C:\Users\HassanRahman\source\repos\qwe`
2. Install recommended extension: "C# Dev Kit"
3. Use Source Control panel (Ctrl+Shift+G) for Git
4. Terminal available for git commands

### GitHub Web (Online Editor)
1. Go to https://github.com/Heyson315/qwe
2. Press `.` (dot key) - Opens VS Code in browser!
3. Or click "Code" > "Codespaces" > "Create codespace"
4. Full development environment in browser

**All three work with the same repository** - Changes in one appear in others after push/pull!

---

## 📋 Common Tasks

### Check Status
```bash
git status              # What changed?
git log --oneline -5    # Recent commits
git branch              # Which branch am I on?
```

### Create Feature Branch
```bash
git checkout develop
git pull
git checkout -b feature/add-payment-system
# Make changes
git add .
git commit -m "feat: Add payment processing"
git push origin feature/add-payment-system
```

### Get Someone Else's Changes
```bash
git checkout develop
git pull origin develop
```

### Undo Mistakes
```bash
# Undo last commit (keep changes)
git reset HEAD~1

# Discard changes to a file
git checkout -- filename.cs

# See what you're about to commit
git diff --staged
```

---

## 🌐 GitHub Features Now Available

### Issues (Task Tracking)
Create issues at: https://github.com/Heyson315/qwe/issues
- Bug reports
- Feature requests  
- Security issues
Templates are ready to use!

### Pull Requests
Review code at: https://github.com/Heyson315/qwe/pulls
- Automatic template applied
- Request reviews
- Discuss changes
- Merge when approved

### Project Board (Kanban)
Set up at: https://github.com/Heyson315/qwe/projects
1. Click "Projects" tab
2. "New project"
3. Choose "Board" template
4. Columns: To Do, In Progress, Done

### Actions (CI/CD)
Will be set up soon for:
- Automatic builds
- Run tests
- Deploy to Azure

---

## 📱 Working From Multiple Locations

### Scenario 1: Work Computer
```bash
cd C:\Users\HassanRahman\source\repos\qwe
git pull origin develop
# Make changes
git commit -am "feat: Update feature"
git push origin develop
```

### Scenario 2: Home Computer
```bash
# First time only
git clone https://github.com/Heyson315/qwe
cd qwe

# Every day
git pull origin develop
# Make changes
git commit -am "feat: Continue work"
git push origin develop
```

### Scenario 3: On the Go (Phone/Tablet)
- Use GitHub mobile app
- View code, issues, PRs
- Review and approve PRs
- Comment on discussions

---

## 🔥 Emergency Commands

### Something Went Wrong
```bash
# See what happened
git status
git log --oneline -10

# Get back to clean state
git checkout develop
git reset --hard origin/develop

# Start over with latest from GitHub
git fetch --all
git reset --hard origin/develop
```

### Merge Conflict
```bash
# Pull caused conflicts
git status  # Shows conflicted files

# Open files, look for:
# <<<<<<< HEAD
# Your changes
# =======
# Their changes
# >>>>>>> branch-name

# Edit files, choose which to keep
git add .
git commit -m "fix: Resolve merge conflicts"
git push
```

---

## 📚 Documentation Files (On GitHub)

All these are now on GitHub:

1. **README.md** - Project overview
   https://github.com/Heyson315/qwe/blob/master/README.md

2. **API_DOCUMENTATION.md** - API reference
   https://github.com/Heyson315/qwe/blob/master/API_DOCUMENTATION.md

3. **DEVELOPMENT_GUIDELINES.md** - Coding standards
   https://github.com/Heyson315/qwe/blob/master/DEVELOPMENT_GUIDELINES.md

4. **GITHUB_SETUP.md** - Detailed GitHub guide
   https://github.com/Heyson315/qwe/blob/master/GITHUB_SETUP.md

---

## ⚙️ Recommended Next Steps

### 1. Enable Branch Protection (5 minutes)
```
Go to: https://github.com/Heyson315/qwe/settings/branches
- Add rule for 'master'
- Add rule for 'develop'
- Require pull request reviews
```

### 2. Create First Issue (2 minutes)
```
Go to: https://github.com/Heyson315/qwe/issues/new/choose
- Choose template
- Fill in details
- Assign to yourself
```

### 3. Set Up Project Board (5 minutes)
```
Go to: https://github.com/Heyson315/qwe/projects
- Create new project
- Add issues to board
- Track progress visually
```

### 4. Invite Collaborators (if working with team)
```
Go to: https://github.com/Heyson315/qwe/settings/access
- Click "Add people"
- Enter GitHub username or email
```

---

## 🎯 Your Current Setup

- ✅ **Repository**: Created and pushed
- ✅ **Branches**: master & develop ready
- ✅ **Documentation**: Complete and online
- ✅ **Templates**: Issues & PRs configured
- ✅ **Structure**: Clean and organized
- ✅ **Tests**: Framework in place

**You can now work from anywhere!**

---

## 💡 Pro Tips

1. **Commit Often** - Small, frequent commits are better than large ones
2. **Descriptive Messages** - Future you will thank present you
3. **Pull Before Push** - Always get latest changes first
4. **Branch for Everything** - Never commit directly to master/develop
5. **Review Your Own PR** - Catch mistakes before others see them

---

## 🆘 Need Help?

### Command Not Working?
```bash
git --version  # Check Git is installed
git status     # Check what's happening
```

### Can't Push?
```bash
git pull --rebase origin develop
git push origin develop
```

### Lost Changes?
```bash
git reflog  # Shows all your actions
# Find the commit hash
git checkout <hash>
```

### Resources
- Git Cheat Sheet: https://education.github.com/git-cheat-sheet-education.pdf
- GitHub Docs: https://docs.github.com/
- Your repo issues: https://github.com/Heyson315/qwe/issues

---

## 🎉 Success!

Your project is now:
- ✅ On GitHub
- ✅ Professionally structured
- ✅ Ready for collaboration
- ✅ Accessible from anywhere
- ✅ Version controlled

**Happy coding!** 🚀

Visit your project: https://github.com/Heyson315/qwe
