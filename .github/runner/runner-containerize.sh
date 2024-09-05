### Bash script for register and run containerized runner
## PAT_RUNNER: ecr_backend_runner — admin:repo_hook, repo, workflow
## paste PAT token for get 1h-registration token for runner. Expires on 2024-12-03 
docker run \
    --restart=always \
    --name="ecrbackend-runner" \
    -v /var/run/docker.sock:/var/run/docker.sock \
    -e REPO_URL="rmolotov/ECRBackend" \
    -e RUNNER_TOKEN=$(curl -sX POST -H "Authorization: token <PAT_RUNNER>" https://api.github.com/repos/rmolotov/ECRBackend/actions/runners/registration-token | jq -r .token) \
    -d ghcr.io/actions/actions-runner:2.319.1 \
    sh -c './config.sh --url https://github.com/${REPO_URL} --token ${RUNNER_TOKEN} --labels local --unattended && ./run.sh'