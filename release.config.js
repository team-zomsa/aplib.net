module.exports = {
  "plugins": [
    "@semantic-release/commit-analyzer",
    "@semantic-release/release-notes-generator",
    "@semantic-release/github",
    [
      "@semantic-release/exec"
    ]
  ],
  "branches": [
    { name: "main" },
    { name: "dev", channel: "dev", prerelease: true }
  ]
}