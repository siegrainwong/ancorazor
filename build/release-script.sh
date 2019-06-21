# release-script for azure release pipeline

cd ~/ancorazor/build

# redirect error log to prevent docker-compose output stderr
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d 2> docker-compose.log
cat docker-compose.log

# this command would failed if there's no images to remove, so redirect this too.
docker rmi $(docker images -a siegrainwong/ancorazor -f "before=siegrainwong/ancorazor:#{Build.BuildNumber}#" -q 2> /dev/null) 2> remove-unused-images.log
cat remove-unused-images.log