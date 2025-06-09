module.exports = {
  content: [
    "./**/*.razor",
    "./wwwroot/index.html",
    // add more paths as needed
  ],
  theme: {
    extend: {
      colors: {
        primary: "#615fff",
        foreground: '#ffffff',
      },
      muted: {
        foreground: "#6b7280",
      },
    },
  },
  plugins: [],
};
