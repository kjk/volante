workspace "Volante"
  configurations { "Debug", "Release" }
  platforms { "Any CPU" }
  flags { "Symbols" }
  defines { "WITH_PATRICIA", "WITH_REPLICATION", "WITH_OLD_BTREE" }
  filter "action:vs*"
    location "vs2015"
  filter "action:monodevelop"
    location "monodevelop"
    defines { "MONO" }
  filter {}

  startproject "Tests"

  project "Volante"
    kind "SharedLib"
    language "C#"
    files { "src/*.cs", "src/impl/*.cs" }
    links { "System" }

  project "Tests"
    kind "ConsoleApp"
    language "C#"
    files { "tests/Tests/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "DirectoryScan"
    kind "ConsoleApp"
    language "C#"
    files { "examples/DirectoryScan/*.cs", "examples/DirectoryScan/Properties/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "Guess"
    kind "ConsoleApp"
    language "C#"
    files { "examples/Guess/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "IpCountry"
    kind "ConsoleApp"
    language "C#"
    files { "examples/IpCountry/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "PropGuess"
    kind "ConsoleApp"
    language "C#"
    files { "examples/PropGuess/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "TestLink"
    kind "ConsoleApp"
    language "C#"
    files { "examples/TestLink/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "TestSOD"
    kind "ConsoleApp"
    language "C#"
    files { "examples/TestSOD/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "TestSSD"
    kind "ConsoleApp"
    language "C#"
    files { "examples/TestSSD/*.cs" }
    links { "System" }
    dependson { "Volante" }

  project "TransparentGuess"
    kind "ConsoleApp"
    language "C#"
    files { "examples/TransparentGuess/*.cs" }
    links { "System" }
    dependson { "Volante" }
