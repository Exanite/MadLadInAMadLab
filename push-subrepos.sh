set -e

git subrepo push TurnBasedTacticsGame/Assets/Plugins/Exanite/Building
git subrepo push TurnBasedTacticsGame/Assets/Plugins/Exanite/Core
git subrepo push TurnBasedTacticsGame/Assets/Plugins/Exanite/Drawing

./clean-subrepos.sh
