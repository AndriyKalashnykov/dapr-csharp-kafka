.DEFAULT_GOAL := help

CURRENTTAG:=$(shell git describe --tags --abbrev=0)
NEWTAG ?= $(shell bash -c 'read -p "Please provide a new tag (currnet tag - ${CURRENTTAG}): " newtag; echo $$newtag')

#help: @ List available tasks
help:
	@clear
	@echo "Usage: make COMMAND"
	@echo "Commands :"
	@grep -E '[a-zA-Z\.\-]+:.*?@ .*$$' $(MAKEFILE_LIST)| tr -d '#' | awk 'BEGIN {FS = ":.*?@ "}; {printf "\033[32m%-20s\033[0m - %s\n", $$1, $$2}'

#clean: @ Cleanup
clean:
	@rm -rf ./publisher/bin/ ./publisher/obj/ ./subscriber/bin/ ./subscriber/obj/

#build: @ Build
build: clean
	cd publisher && dotnet build publisher.csproj && cd ..
	cd subscriber && dotnet build subscriber.csproj && cd ..

#release: @ Create and push a new tag
release:
	$(eval NT=$(NEWTAG))
	@echo -n "Are you sure to create and push ${NT} tag? [y/N] " && read ans && [ $${ans:-N} = y ]
	@echo ${NT} > ./version.txt
	@git add -A
	@git commit -a -s -m "Cut ${NT} release"
	@git tag ${NT}
	@git push origin ${NT}
	@git push
	@echo "Done."

#version: @ Print current version(tag)
version:
	@echo $(shell git describe --tags --abbrev=0)

#runp: @ Run publisher
runp: build
	dotnet run --project publisher/publisher.csproj $(shell dirname $(realpath $(firstword $(MAKEFILE_LIST))))/kafka.properties

#runs: @ Run subscriber
runs: build
	dotnet run --project subscriber/subscriber.csproj $(shell dirname $(realpath $(firstword $(MAKEFILE_LIST))))/kafka.properties

# upgrade outdated https://github.com/NuGet/Home/issues/4103
#upgrade: @ Upgrade outdated packages
upgrade:
	@cd publisher && dotnet list package --outdated | grep -o '> \S*' | grep '[^> ]*' -o | xargs --no-run-if-empty -L 1 dotnet add package
	@cd publisher && dotnet list package --outdated | grep -o '> \S*' | grep '[^> ]*' -o | xargs --no-run-if-empty -L 1 dotnet add package

