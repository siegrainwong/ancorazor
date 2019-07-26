# release-script for azure release pipeline
cd ~/ancorazor/build

# redirect error log to prevent docker-compose output stderr
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d 2> docker-compose.log
cat docker-compose.log

# warm up
curl https://siegrain.wang -o warm-up.log