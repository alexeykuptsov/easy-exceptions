import os


def get_version():
    with open('version.txt', 'r') as f:
        return f.read() + '.' + (os.environ.get('BUILD_COUNTER') or '0')
