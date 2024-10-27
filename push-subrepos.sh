set -e

git subrepo push Wargames2024/Assets/Plugins/Exanite/Building
git subrepo push Wargames2024/Assets/Plugins/Exanite/Core
git subrepo push Wargames2024/Assets/Plugins/Exanite/Drawing

./clean-subrepos.sh
