let
  nixpkgsVer = "24.05";
  pkgs = import (fetchTarball "https://github.com/NixOS/nixpkgs/tarball/nixos-${nixpkgsVer}") { config = {}; overlays = []; };
  sdk = pkgs.dotnetCorePackages.sdk_8_0;
  DOTNET_ROOT = "${sdk.out}";
  DOTNET_HOST_PATH = "${DOTNET_ROOT}/bin/dotnet";
in pkgs.mkShell {
  name = "snakes";

  inherit DOTNET_ROOT;
  inherit DOTNET_HOST_PATH;

  buildInputs = with pkgs; [
    sdk
    git
    git-extras
  ];
}
