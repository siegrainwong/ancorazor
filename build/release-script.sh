set -ex
cd #{ScriptDirectory}#

docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d

# Mark: The input device is not a TTY
# https://stackoverflow.com/questions/43099116/error-the-input-device-is-not-a-tty

# remove previous version of ancorazor images
# redirect error log to a txt file because this command will cause an error if there are no images to remove
docker rmi $(docker images -a siegrainwong/ancorazor \
				-f "before=siegrainwong/ancorazor:#{Build.BuildNumber}#" -q) 2>ignore.txt

rm -rf ignore.txt