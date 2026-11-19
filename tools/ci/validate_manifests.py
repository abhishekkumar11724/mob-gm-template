import os
import json
import sys

def validate_manifests():
    root = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
    packages_dir = os.path.join(root, "packages")
    manifest_path = os.path.join(root, "sample-game", "Packages", "manifest.json")
    matrix_path = os.path.join(root, "version-matrix.json")
    
    errors = []
    
    if not os.path.exists(matrix_path):
        errors.append(f"Missing version-matrix.json at {matrix_path}")
        return errors
        
    with open(matrix_path, "r") as f:
        matrix = json.load(f)
        
    matrix_packages = matrix.get("packages", {})
    
    if not os.path.exists(manifest_path):
        errors.append(f"Missing manifest.json at {manifest_path}")
    else:
        with open(manifest_path, "r") as f:
            manifest = json.load(f)
        manifest_deps = manifest.get("dependencies", {})
    
    found_packages = []
    for item in os.listdir(packages_dir):
        pkg_path = os.path.join(packages_dir, item)
        if not os.path.isdir(pkg_path) or not item.startswith("com.nimbus."):
            continue
            
        found_packages.append(item)
        pkg_json_path = os.path.join(pkg_path, "package.json")
        
        if not os.path.exists(pkg_json_path):
            errors.append(f"Package {item} is missing package.json")
            continue
            
        with open(pkg_json_path, "r") as f:
            pkg_json = json.load(f)
            
        pkg_name = pkg_json.get("name")
        pkg_version = pkg_json.get("version")
        
        if pkg_name != item:
            errors.append(f"Package folder {item} does not match name in package.json: {pkg_name}")
            
        if item not in matrix_packages:
            errors.append(f"Package {item} is missing from version-matrix.json")
        elif pkg_version != matrix_packages[item]:
            errors.append(f"Package {item} version {pkg_version} does not match version-matrix.json ({matrix_packages[item]})")
            
        if manifest_path and item not in manifest_deps:
            errors.append(f"Package {item} is missing from sample-game/Packages/manifest.json")
            
    for pkg_name in matrix_packages:
        if pkg_name not in found_packages:
            errors.append(f"Package {pkg_name} in version-matrix.json was not found in packages/ folder")
            
    return errors

if __name__ == "__main__":
    validation_errors = validate_manifests()
    if validation_errors:
        for error in validation_errors:
            print(f"ERROR: {error}")
        sys.exit(1)
    else:
        print("Manifest validation successful")
        sys.exit(0)
