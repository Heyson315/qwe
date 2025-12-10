# Marketing Website - GitHub Pages

This is the public-facing marketing website for HHR CPA platform, hosted on GitHub Pages.

## Live Site

- **GitHub Pages URL**: https://heyson315.github.io/qwe/
- **Custom Domain** (optional): Configure in repository settings

## Structure

```
docs/
├── index.html          # Main marketing page
├── styles.css          # Styling
├── script.js           # JavaScript functionality
├── CNAME              # Custom domain configuration (optional)
└── README.md          # This file
```

## Features

### Sections
1. **Hero** - Main call-to-action
2. **Features** - Platform capabilities
3. **Services** - CPA services offered
4. **Demo** - Link to live demo (Docker container)
5. **Technology** - Tech stack showcase
6. **Contact** - Contact form and information

### Integrations
- Links to SharePoint team portal
- GitHub repository
- Docker demo application
- Documentation

## Local Development

To test locally:

```bash
# Option 1: Python HTTP Server
cd docs
python -m http.server 8000
# Visit http://localhost:8000

# Option 2: Node.js HTTP Server
npx http-server docs -p 8000

# Option 3: Visual Studio Code Live Server
# Install "Live Server" extension
# Right-click index.html → "Open with Live Server"
```

## Deployment

### Enable GitHub Pages

1. Go to repository settings: https://github.com/Heyson315/qwe/settings/pages
2. Under "Source", select branch: `copilot/add-feature-to-vigilant-octo-engine` (or `main`)
3. Select folder: `/docs`
4. Click "Save"

Your site will be published automatically at: `https://heyson315.github.io/qwe/`

### Custom Domain (GoDaddy)

If you want to use a custom domain:

1. **Add CNAME file** to this directory with your domain:
   ```
   echo "www.hhrcpa.us" > docs/CNAME
   ```

2. **Configure DNS in GoDaddy**:
   - Add CNAME record:
     - Type: CNAME
     - Name: www
     - Value: heyson315.github.io
   
   - Add A records for apex domain:
     - 185.199.108.153
     - 185.199.109.153
     - 185.199.110.153
     - 185.199.111.153

3. **Enable HTTPS** in GitHub repository settings

## Customization

### Update Contact Information

Edit `index.html`:
```html
<p><strong>Email:</strong> your-email@domain.com</p>
<p><strong>Phone:</strong> (123) 456-7890</p>
```

### Change Colors

Edit `styles.css`:
```css
:root {
    --primary-color: #0066cc;    /* Your brand color */
    --secondary-color: #00b894;  /* Accent color */
}
```

### Add Demo Video

Replace the placeholder in `index.html`:
```html
<div class="video-placeholder">
    <iframe src="YOUR_VIDEO_URL" frameborder="0" allowfullscreen></iframe>
</div>
```

### Connect to Backend API

Edit `script.js`:
```javascript
const API_ENDPOINT = 'https://your-domain.com/api';
// or for Azure: 'https://your-app.azurewebsites.net/api'
```

## Integration with Main Project

### Link to Docker Demo

The site includes a link to the Docker demo at `http://localhost:8080`. Users need to:
1. Have Docker Desktop running
2. Run `docker-compose up -d` from project root
3. Click "Launch Demo App" button

### SharePoint Integration

Team members can access:
- **Public site**: GitHub Pages (this site)
- **Team portal**: SharePoint link in navigation
- **Project docs**: SharePoint document library

### Marketing Workflow

```
┌─────────────────┐
│  GitHub Pages   │ ← Public marketing
│  (Static Site)  │
└────────┬────────┘
         │
         ├─────────→ Lead Form Submission
         │
         ▼
┌─────────────────┐
│  Docker App     │ ← Demo & Testing
│  (localhost)    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  SharePoint     │ ← Internal collaboration
│  (Team Portal)  │
└─────────────────┘
```

## SEO Optimization

### Meta Tags

Already included in `index.html`:
```html
<meta name="description" content="HHR CPA - Professional accounting services">
<title>HHR CPA - Professional Accounting Services</title>
```

### Add More SEO

```html
<meta name="keywords" content="CPA, accounting, tax preparation, bookkeeping">
<meta property="og:title" content="HHR CPA">
<meta property="og:description" content="Professional accounting services">
<meta property="og:image" content="https://your-domain.com/og-image.jpg">
```

## Analytics

To add Google Analytics:

1. Get tracking ID from Google Analytics
2. Add to `index.html` before `</head>`:

```html
<!-- Google Analytics -->
<script async src="https://www.googletagmanager.com/gtag/js?id=GA_MEASUREMENT_ID"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'GA_MEASUREMENT_ID');
</script>
```

## Performance

Current optimizations:
- ✓ Minimal dependencies (no frameworks)
- ✓ Inline critical CSS
- ✓ Lazy loading for animations
- ✓ Optimized images (SVG backgrounds)

### Future Improvements
- [ ] Add image compression
- [ ] Implement PWA features
- [ ] Add service worker for offline support
- [ ] Minify CSS/JS for production

## Accessibility

Features:
- Semantic HTML5 elements
- ARIA labels where needed
- Keyboard navigation support
- Responsive design for all devices
- High contrast ratios for text

## Browser Support

Tested on:
- ✓ Chrome 90+
- ✓ Firefox 88+
- ✓ Safari 14+
- ✓ Edge 90+

## Troubleshooting

### Site Not Updating

GitHub Pages can take a few minutes to update:
1. Check GitHub Actions tab for build status
2. Clear browser cache (Ctrl+F5)
3. Try incognito/private browsing mode

### Custom Domain Not Working

1. Verify CNAME file is in `/docs` folder
2. Check DNS propagation: https://dnschecker.org
3. Wait 24-48 hours for DNS to propagate
4. Ensure HTTPS is enforced in settings

### Form Not Submitting

The contact form requires the backend API:
1. Run Docker container: `docker-compose up -d`
2. Implement `/api/contact` endpoint in ASP.NET MVC app
3. Update `API_ENDPOINT` in `script.js`

## Resources

- [GitHub Pages Documentation](https://docs.github.com/en/pages)
- [Custom Domain Setup](https://docs.github.com/en/pages/configuring-a-custom-domain-for-your-github-pages-site)
- [Markdown Guide](https://guides.github.com/features/mastering-markdown/)

## Support

For issues with the website:
- GitHub Issues: https://github.com/Heyson315/qwe/issues
- Email: contact@hhrcpa.us
